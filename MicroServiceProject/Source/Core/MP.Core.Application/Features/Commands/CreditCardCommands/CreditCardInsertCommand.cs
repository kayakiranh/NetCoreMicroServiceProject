using AutoMapper;
using MediatR;
using MP.Core.Application.DataTransferObjects;
using MP.Core.Application.Repositories;
using MP.Core.Application.Wrapper;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MP.Core.Application.Features.Commands.CreditCardCommands
{
    public class CreditCardInsertCommand : IRequest<ApiResponse>
    {
        public CreditCardDto CreditCardDto { get; set; }

        public class CreditCardInsertCommandHandler : IRequestHandler<CreditCardInsertCommand, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly IMapper _mapper;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CreditCardInsertCommandHandler(ICreditCardRepository creditCardRepository, IMapper mapper, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _creditCardRepository = creditCardRepository;
                _mapper = mapper;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CreditCardInsertCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    CreditCard creditCard = _mapper.Map<CreditCard>(request.CreditCardDto);
                    CreditCard insertResponse = await _creditCardRepository.Insert(creditCard);

                    if (insertResponse.Id < 1)
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardInsertCommand Error", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardInsertCommand Error");
                    }
                    else
                    {
                        _cacheRepository.SetData(insertResponse.Name, insertResponse);
                        _logger.Insert(LogTypes.Information, "CreditCardInsertCommand Success");
                        response = ApiResponse.SuccessResponse(insertResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CreditCardInsertCommand Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CreditCardInsertCommand Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}