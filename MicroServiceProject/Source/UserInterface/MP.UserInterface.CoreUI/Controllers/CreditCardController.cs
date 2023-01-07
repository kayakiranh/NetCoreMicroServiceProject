using Microsoft.AspNetCore.Mvc;

namespace MP.UserInterface.CoreUI.Controllers
{
    public class CreditCardController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Detail(string cardName)
        {
            return View();
        }
    }
}