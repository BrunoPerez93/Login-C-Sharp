using LoginApp.Data;
using LoginApp.Models;
using LoginApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace LoginApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDBContext _dbContext;
        public LoginController(AppDBContext appDB)
        {
            _dbContext = appDB;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            if (userViewModel.Password != userViewModel.ConfirmPassword)
            {
                ViewData["Mensaje"] = "The Password Dont Match";
                return View();
            }

            User user = new User()
            {
                Name = userViewModel.Name,
                Email = userViewModel.Email,
                Password = userViewModel.Password,
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            if (user.IdUser != 0)
            {
                return RedirectToAction("Login", "Login");
            }

            ViewData["Mensaje"] = "Cant Create The User";

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            User? user = await _dbContext.Users
                         .Where(u =>
                            u.Email == loginViewModel.Email &&
                            u.Password == loginViewModel.Password
                            ).FirstOrDefaultAsync();

            if (user == null)
            {
                ViewData["Mensaje"] = "The email or password are incorrect.";

                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
