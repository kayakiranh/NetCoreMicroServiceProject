using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MP.UserInterface.CoreUI.Controllers
{
    [Authorize]
    public class ApplyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate()
        {
            return View();
        }
    }
}