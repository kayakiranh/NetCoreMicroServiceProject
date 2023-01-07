using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Application.ViewModels;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using MP.Infrastructure.Helper;
using System.Threading.Tasks;

namespace MP.Api.IdentityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;
        private readonly ILoggerRepository _loggerRepository;

        public AuthController(ICustomerRepository customerRepository, ILoggerRepository loggerRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
            _loggerRepository = loggerRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            Customer customer = await _customerRepository.Login(model.Email, model.Password);
            if (customer.Id == 0)
            {
                _loggerRepository.Insert(LogTypes.Information, "Customer Not Found", null, model.Email, model.Password);
                return BadRequest("Customer Not Found");
            }
            return Ok(customer);
        }

        [Authorize]
        [HttpPost("check")]
        public async Task<IActionResult> Check([FromBody] string token)
        {
            Customer customer = await _customerRepository.GetByToken(token);
            if (customer.Id == 0)
            {
                _loggerRepository.Insert(LogTypes.Warning, "Token Not Found", null, token);
                return BadRequest("Token Not Found");
            }

            return Ok(customer);
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string token)
        {
            Customer customer = await _customerRepository.RefreshToken(token);
            if (customer.Id == 0)
            {
                _loggerRepository.Insert(LogTypes.Warning, "Token Not Found", null, token);
                return BadRequest("Token Not Found");
            }

            return Ok(customer);
        }

        [HttpPost("test-user")]
        public async Task<IActionResult> GetSwaggerToken()
        {
            _loggerRepository.Insert(LogTypes.Information, "Swagger User Generated");
            Customer customer = await _customerRepository.Login(_configuration["JWT:SwaggerUserEmail"], _configuration["JWT:SwaggerUserPassword"]);
            if (customer.Id == 0)
            {
                customer = await _customerRepository.Insert(new Customer
                {
                    EmailAddress = _configuration["JWT:SwaggerUserEmail"],
                    FullName = "Swagger Test User",
                    IdentityNumber = "1234567890",
                    MonthlyIncome = 12500,
                    Password = _configuration["JWT:SwaggerUserPassword"].Encrypt(),
                    Phone = "+905551112233",
                    Status = (int)EntityStatus.Active,
                    Token = ""
                });
            }
            customer = await _customerRepository.Login(_configuration["JWT:SwaggerUserEmail"], _configuration["JWT:SwaggerUserPassword"]);
            return Ok(customer);
        }
    }
}