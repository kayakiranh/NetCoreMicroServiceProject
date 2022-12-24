using MP.Core.Domain.Entities;
using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MP.Core.Application.Repositories;
using System.Threading;
using MP.Core.Application.DataTransferObjects;
using MP.Core.Domain.Enums;
using MP.Core.Application.Wrapper;

namespace MP.Core.Application.Features.Commands.CreditCardCommands
{
    public class CreditCardUpdateCommand : IRequest<ApiResponse>
    {
        public CreditCardDto CreditCardDto { get; set; }

        public class CreditCardUpdateCommandHandler : IRequestHandler<CreditCardUpdateCommand, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly IMapper _mapper;
            private readonly ILoggerRepository _logger;

            public CreditCardUpdateCommandHandler(ICreditCardRepository creditCardRepository, IMapper mapper, ILoggerRepository logger)
            {
                _creditCardRepository = creditCardRepository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CreditCardUpdateCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    CreditCard CreditCard = _mapper.Map<CreditCard>(request.CreditCardDto);
                    CreditCard UpdateResponse = await _creditCardRepository.Update(CreditCard);

                    if (UpdateResponse.Id < 1)
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardUpdateCommand Error", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardUpdateCommand Error");
                    }
                    else
                    {                        
                        _logger.Insert(LogTypes.Information, "CreditCardUpdateCommand Success");
                        response = ApiResponse.SuccessResponse(UpdateResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CreditCardUpdateCommand Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CreditCardUpdateCommand Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}