using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Application.ViewModels;

namespace MP.UserInterface.CoreUI.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            return View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            return View();
        }

        [Authorize]
        public IActionResult Applications()
        {
            return View();
        }
    }
}