using Microsoft.AspNetCore.Mvc;

namespace MP.UserInterface.CoreUI.ViewComponents
{
    public class LoginViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("LoginViewComponent");
        }
    }
}