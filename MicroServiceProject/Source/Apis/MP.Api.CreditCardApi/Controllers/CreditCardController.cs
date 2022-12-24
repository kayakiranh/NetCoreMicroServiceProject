using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Application.Features.Commands.CreditCardCommands;
using MP.Core.Application.Features.Queries.CreditCardQueries;
using MP.Core.Application.Wrapper;
using System.Threading.Tasks;

namespace MP.Api.CustomerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CreditCardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreditCardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("insert")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Insert([FromBody] CreditCardInsertCommand command)
        {
            ApiResponse response = await _mediator.Send(command);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("remove")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Remove([FromBody] CreditCardRemoveCommand command)
        {
            ApiResponse response = await _mediator.Send(command);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Update([FromBody] CreditCardUpdateCommand command)
        {
            ApiResponse response = await _mediator.Send(command);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        //public async Task<List<CreditCard>> ListByType(CreditCardTypes creditCardType)

        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> List(CreditCardListQuery query)
        {
            ApiResponse response = await _mediator.Send(query);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("by-bank")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> ListByBank(CreditCardListByBankQuery query)
        {
            ApiResponse response = await _mediator.Send(query);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("by-financial-score")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> ListByFinancialScore(CreditCardListByFinancialScoreQuery query)
        {
            ApiResponse response = await _mediator.Send(query);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("by-type")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> ListByType(CreditCardListByTypeQuery query)
        {
            ApiResponse response = await _mediator.Send(query);

            if (!response.Status) return BadRequest(response);

            return Ok(response);
        }
    }
}