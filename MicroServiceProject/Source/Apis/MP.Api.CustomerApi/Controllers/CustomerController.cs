using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Application.Features.Commands.CustomerCommands;
using MP.Core.Application.Features.Queries.CustomerQueries;
using MP.Core.Application.Wrapper;
using System.Threading.Tasks;

namespace MP.Api.CustomerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("insert")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Insert([FromBody] CustomerInsertCommand command)
        {
            ApiResponse response = await _mediator.Send(command);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("remove")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Remove([FromBody] CustomerRemoveCommand command)
        {
            ApiResponse response = await _mediator.Send(command);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Update([FromBody] CustomerUpdateCommand command)
        {
            ApiResponse response = await _mediator.Send(command);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> List(CustomerListQuery query)
        {
            ApiResponse response = await _mediator.Send(query);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("by-token")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> ByToken(CustomerGetByTokenQuery query)
        {
            ApiResponse response = await _mediator.Send(query);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("by-identity")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> ByIdentity(CustomerGetByIdentityNumberQuery query)
        {
            ApiResponse response = await _mediator.Send(query);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }
    }
}