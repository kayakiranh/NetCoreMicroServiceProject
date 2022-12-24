using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Application.ViewModels;
using MP.Core.Domain.Entities;
using MP.Infrastructure.Helper;
using System.Threading.Tasks;
using MP.Core.Domain.Enums;

namespace MP.Api.FinancialRatingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialRatingController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILoggerRepository _loggerRepository;
        private readonly IMailerRepository _mailerRepository;
        private readonly IConfiguration _configuration;

        public FinancialRatingController(ICustomerRepository customerRepository, ILoggerRepository loggerRepository, IMailerRepository mailerRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _loggerRepository = loggerRepository;
            _mailerRepository = mailerRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] FinancialRatingViewModel model)
        {
            if (model.IdentityNumber.StringSecurityValidation() == "" || model.MonthlyIncome < 1)
            {
                _loggerRepository.Insert(LogTypes.Information, "ConnectAnyFinancialService BadRequest", null, model);
                return BadRequest();
            }

            Customer customer = await _customerRepository.GetByIdentityNumber(model.IdentityNumber);
            if (customer.Id == 0)
            {
                _loggerRepository.Insert(LogTypes.Information, "ConnectAnyFinancialService BadRequest", null, customer, model);
                return BadRequest();
            }
            FinancialApiResponse fakeApiResponse = await FakeApiClient(model.IdentityNumber);
            if (fakeApiResponse == null)
            {
                _loggerRepository.Insert(LogTypes.Error, "ConnectAnyFinancialService BadRequest", null, customer, model);
                _mailerRepository.SendToAdmin(EmailTemplates.FinancialApiError, model);
            }

            //Fake calculate
            double score = 0.5;
            if (fakeApiResponse.HaveCar) score++;
            if (fakeApiResponse.HaveHouse) score++;
            if (fakeApiResponse.HaveCreditCard && !fakeApiResponse.InsideBlackList) score++;
            if (fakeApiResponse.TotalWorkDay > 1000) score += 0.5;
            if (fakeApiResponse.TotalIncome > 12000) score += 0.5;
            if (fakeApiResponse.MonthlySalary < model.MonthlyIncome) score += 0.5;
            else score += 0.25;

            return Ok(score);
        }

        private async Task<FinancialApiResponse> FakeApiClient(string identity)
        {
            _loggerRepository.Insert(LogTypes.Information, "FakeApiClient call", null, identity);
            FinancialApiRequest request = new FinancialApiRequest
            {
                ApiKey = _configuration.GetSection("FinancialApi:ApiKey").Value,
                ApiSecretKey = _configuration.GetSection("FinancialApi:ApiSecretKey").Value,
                Identity = identity
            };

            return await Task.FromResult(new FinancialApiResponse
            {
                FirstName = "Hüseyin",            
                LastName = "Kayakıran",
                Address = "Fake Address",
                Identity = "123456789",
                TotalWorkDay = 4000,
                MonthlySalary = 10000,
                TotalIncome = 12000,
                HaveCar = true,
                HaveHouse = true,
                HaveCreditCard = true,
                InsideBlackList = false
            });
        }

        private class FinancialApiResponse
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string Identity { get; set; }
            public int TotalWorkDay { get; set; }
            public decimal MonthlySalary { get; set; }
            public decimal TotalIncome { get; set; }
            public bool HaveCar { get; set; }
            public bool HaveHouse { get; set; }
            public bool HaveCreditCard { get; set; }
            public bool InsideBlackList { get; set; }
        }

        private class FinancialApiRequest
        {
            public string Identity { get; set; }
            public string ApiKey { get; set; }
            public string ApiSecretKey { get; set; }
        }
    }
}