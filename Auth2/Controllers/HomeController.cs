using Auth2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("login")]
        public IActionResult login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Validate(string username,string password,string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            if (username == "chandan" && password == "tomar")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username",username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim(ClaimTypes.Name, username));
                //claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                var claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimprincipals = new ClaimsPrincipal(claimsidentity);
                await HttpContext.SignInAsync(claimprincipals);
                return Redirect(returnUrl);
            }
               
            else
            {
                ViewData["error"] = "Login failed ,Please use correct username password ";
            }
                return View("login");
        }
        public IActionResult denied()
        {
            
            return View();
        }
        [Authorize]
        public IActionResult logout()
        {
            HttpContext.SignOutAsync();
            return Redirect(@"https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue=https://localhost:5001");
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Secured()
        {
            var token = await HttpContext.GetTokenAsync("id_token");
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
