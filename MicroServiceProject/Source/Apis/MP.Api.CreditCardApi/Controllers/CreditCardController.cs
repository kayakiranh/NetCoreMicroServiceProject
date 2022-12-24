﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MP.Core.Application.Features.Commands.CreditCardCommands;
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
    }
}