using Microsoft.AspNetCore.Mvc;

namespace MP.UserInterface.CoreUI.Components.LoginViewComponent
{
    [ViewComponent(Name = "LoginViewComponent")]
    public class LoginViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("LoginViewComponent");
        }
    }
}