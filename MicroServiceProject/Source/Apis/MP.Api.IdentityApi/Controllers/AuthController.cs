using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Application.ViewModels;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System.Threading.Tasks;

namespace MP.Api.IdentityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;

        public AuthController(ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            Customer customer = await _customerRepository.Login(model.Email, model.Password);
            if (customer.Id == 0) return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpPost("check")]
        public async Task<IActionResult> Check([FromBody] string token)
        {
            Customer customer = await _customerRepository.GetByToken(token);
            if (customer.Id == 0) return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string token)
        {
            Customer customer = await _customerRepository.GetByToken(token);
            if (customer.Id == 0) return BadRequest();

            Customer updateCustomer = await _customerRepository.Login(customer.EmailAddress, customer.Password);
            if (updateCustomer.Id == 0) return BadRequest();

            return Ok();
        }

        [HttpPost("test-user")]
        public async Task<IActionResult> GetSwaggerToken()
        {
            Customer customer = await _customerRepository.Login(_configuration["JWT:SwaggerUserEmail"], _configuration["JWT:SwaggerUserPassword"]);
            if (customer.Id == 0)
            {
                customer = await _customerRepository.Insert(new Customer
                {
                    EmailAddress = _configuration["JWT:SwaggerUserEmail"],
                    FullName = "Swagger Test User",
                    IdentityNumber = "1234567890",
                    MonthlyIncome = 12500,
                    Password = _configuration["JWT:SwaggerUserPassword"],
                    Phone = "+905551112233",
                    Status = (int)EntityStatus.Active,
                    Token = ""
                });
                customer = await _customerRepository.Login(customer.EmailAddress, customer.Password);
                if (customer.Id > 0) return Ok(customer);
            }
            return BadRequest();
        }
    }
}