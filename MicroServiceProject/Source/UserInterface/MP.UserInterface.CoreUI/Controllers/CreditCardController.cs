using Microsoft.AspNetCore.Mvc;

namespace MP.UserInterface.CoreUI.Controllers
{
    [Route("Home")]
    public class CreditCardController : Controller
    {
        [Route("/")]
        public IActionResult List()
        {
            return View();
        }

        [Route("~/CreditCard/{cardName}")]
        public IActionResult Detail(string cardName)
        {
            return View();
        }
    }
}