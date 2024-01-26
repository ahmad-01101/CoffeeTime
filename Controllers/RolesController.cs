using CoffeeTime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeTime.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoles(string id, bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"!غير موجود ({id})";
                return View("NotFound");
            }

            var model = new EditRolesModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoles(EditRolesModel editRolesModel, bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(editRolesModel.Id);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"!غير موجود ({editRolesModel.Id})";
                    return View("NotFound");
                }
                else
                {
                    role.Name = editRolesModel.RoleName;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(editRolesModel);
                    }

                    return RedirectToAction(nameof(EditRoles), new { isSuccess = true });
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId, IdentityRole roleName)
        {

            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);
            ViewBag.roleName = role.Name;

            if (role == null)
            {
                ViewBag.ErrorMessage = $"!غير موجود ({roleId})";
                return View("NotFound");
            }

            var model = new List<UserRoleModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleModel = new UserRoleModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleModel.IsSelected = true;
                }
                else
                {
                    userRoleModel.IsSelected = false;
                }
                model.Add(userRoleModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"!غير موجود ({roleId})";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;

                    else
                        return RedirectToAction(nameof(EditRoles), new { Id = roleId, isSuccess = true });
                }
            }


            return RedirectToAction(nameof(EditRoles), new { Id = roleId, isSuccess = true });

        }
    }
}
