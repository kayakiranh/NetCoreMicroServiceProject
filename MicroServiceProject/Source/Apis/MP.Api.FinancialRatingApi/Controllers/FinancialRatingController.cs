using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Application.ViewModels;
using System;
using System.Threading.Tasks;

namespace MP.Api.FinancialRatingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialRatingController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IConfiguration _configuration;

        public FinancialRatingController(ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] FinancialRatingViewModel model)
        {
            double financialScore = await ConnectAnyFinancialService(model);
            return Ok(financialScore);
        }

        private async Task<double> ConnectAnyFinancialService(FinancialRatingViewModel model)
        {
            //Fake response
            Random rnd = new Random();
            string firstName = "Hüseyin";
            string lastName = "Kayakıran";
            string address = "Fake Address";
            int totalWorkDay = 4000;
            decimal monthlySalary = 10000;
            decimal totalIncome = 12000000;
            bool haveCar = true;
            bool haveHouse = true;
            bool haveCreditCard = true;
            bool insideBlackList = false;

            //Fake calculate
            double score = 0.5;
            if (haveCar) score++;
            if (haveHouse) score++;
            if (haveCreditCard && !insideBlackList) score++;
            if (totalWorkDay > 1000) score = score + 0.5;
            if (totalIncome > 1000000) score = score + 0.5;
            if (monthlySalary < model.MonthlyIncome) score = score + 0.5;
            else score = score + 0.5;
            return score;
        }
    }
}