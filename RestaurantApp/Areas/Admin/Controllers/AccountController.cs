using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Areas.Admin.Models;
using RestaurantApp.Data;

namespace RestaurantApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class AccountController : Controller
    {
        RoleManager<AppRole> roleManager;
        UserManager<AppUser> userManager;
        public AccountController(RoleManager<AppRole>_roleManager,UserManager<AppUser>_userManager) 
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if(!ModelState.IsValid) 
                return View(model);
            var user = userManager.Users.FirstOrDefault(s => s.Email == model.Email);
            if (user != null)
            {
                var passCheck = await userManager.CheckPasswordAsync(user, model.Password);
                if (passCheck)
                {
                    var userRoles=await userManager.GetRolesAsync(user);
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email)
                        };
                    foreach(var item in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item));
                    }
                    ClaimsIdentity CI = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    CI.AddClaims(claims);
                    ClaimsPrincipal cp = new ClaimsPrincipal();
                    cp.AddIdentity(CI);
                    AuthenticationProperties auth = new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.Now.AddMinutes(15)
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp, auth);
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }
           
        }

        public string Logout()
        {
            return "logout";
        }
    }
}

