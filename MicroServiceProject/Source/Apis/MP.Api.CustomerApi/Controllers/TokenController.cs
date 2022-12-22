using MediatR;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Application.Features.Queries;
using MP.Core.Application.Wrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Api.CustomerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TokenController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public TokenController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        public async Task<string> Index()
        {
            CustomerLoginQuery customerLoginQuery = new CustomerLoginQuery
            {
                Email = "kayakiranh@gmail.com",
                Password = "1234567890"
            };
            ApiResponse apiResponse = await _mediatr.Send(customerLoginQuery);
            Customer user = JsonConvert.DeserializeObject<Customer>(apiResponse.Result.ToString());
            return new string(user.AccessToken);
        }
    }
}