using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.ViewModels;
using MP.Core.Domain.Entities;
using MP.UserInterface.CoreUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MP.UserInterface.CoreUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            string response = string.Empty;
            string url = new GatewayClientHelper(_configuration).SetUrl(GatewayClientHelper.Actions.AuthLogin);
            using (HttpClient httpClient = new HttpClient())
            {
                Dictionary<string, string> loginValues = new Dictionary<string, string>
                {
                   { "Email", model.Email },
                   { "Password", model.Password }
                };
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(loginValues);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, formUrlEncodedContent);
                response = await httpResponseMessage.Content.ReadAsStringAsync();
            }

            if (!string.IsNullOrEmpty(response))
            {
                Customer customer = JsonConvert.DeserializeObject<Customer>(response);
                CookieOptions option = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddHours(4)
                };
                Response.Cookies.Append("token", customer.Token, option);
                return Ok();
            }

            return BadRequest();
        }

        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return RedirectToAction("List", "CreditCard");
        }

        [Authorize]
        public IActionResult Applications()
        {
            return View();
        }
    }
}