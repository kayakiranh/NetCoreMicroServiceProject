using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.Features.Commands.CustomerCommands;
using MP.Core.Application.Wrapper;
using System.Threading.Tasks;

namespace MP.Api.CustomerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TokenController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public TokenController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost("swagger")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] CustomerLoginCommand command)
        {
            command.Email = _configuration.GetSection("JWT:SwaggerUserEmail").Value;
            command.Password = _configuration.GetSection("JWT:SwaggerUserPassword").Value;
            ApiResponse response = await _mediator.Send(command);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }
    }
}