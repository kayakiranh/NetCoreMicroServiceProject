using MP.Core.Domain.Entities;
using System;
using System.Threading.Tasks;
using MediatR;
using MP.Core.Application.Repositories;
using System.Threading;
using MP.Core.Domain.Enums;
using MP.Core.Application.Wrapper;
using System.Collections.Generic;
using System.Linq;

namespace MP.Core.Application.Features.Queries.CreditCardQueries
{
    public class CreditCardListByBankQuery : IRequest<ApiResponse>
    {
        public BankNames BankName { get; set; }

        public class CreditCardListByBankQueryHandler : IRequestHandler<CreditCardListByBankQuery, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CreditCardListByBankQueryHandler(ICreditCardRepository creditCardRepository, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _creditCardRepository = creditCardRepository;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CreditCardListByBankQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    List<CreditCard> listByBankResponse = await _creditCardRepository.ListByBank(request.BankName);
                    if (!listByBankResponse.Any())
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardListByBankQuery DataNotFound", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardListByBankQuery DataNotFound");
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Information, "CreditCardListByBankQuery Success");
                        response = ApiResponse.SuccessResponse(listByBankResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CreditCardListByBankQuery Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CreditCardListByBankQuery Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}