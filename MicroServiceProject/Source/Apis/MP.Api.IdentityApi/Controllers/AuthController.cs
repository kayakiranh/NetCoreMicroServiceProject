using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MP.Core.Application.Repositories;
using MP.Core.Application.ViewModels;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            string token = GenerateToken(model.Email);
            _customerRepository.UpdateToken(customer.Id, token);

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

            string newToken = GenerateToken(customer.EmailAddress);
            _customerRepository.UpdateToken(customer.Id, newToken);

            return Ok();
        }

        [HttpPost("get-swagger-token")]
        public async Task<IActionResult> GetSwaggerToken()
        {
            Customer customer = await _customerRepository.Login(_configuration["JWT:SwaggerUserEmail"], _configuration["JWT:SwaggerUserPassword"]);
            if (customer.Id == 0)
            {
                Random rnd = new Random();
                customer = await _customerRepository.Insert(new Customer
                {
                    EmailAddress = _configuration["JWT:SwaggerUserEmail"],
                    FullName = "Swagger Test User",
                    IdentityNumber = "1234567890",
                    MonthlyIncome = rnd.Next(3000, 50000),
                    Password = _configuration["JWT:SwaggerUserPassword"],
                    Phone = "+905551112233",
                    Status = (int)EntityStatus.Active,
                    Token = ""
                });
            }

            string token = GenerateToken(_configuration["JWT:SwaggerUserEmail"]);
            _customerRepository.UpdateToken(customer.Id, token);

            return Ok();
        }

        private string GenerateToken(string email)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}