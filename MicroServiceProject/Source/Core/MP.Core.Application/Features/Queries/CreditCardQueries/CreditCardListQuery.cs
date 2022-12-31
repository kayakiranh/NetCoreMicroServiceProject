using MediatR;
using MP.Core.Application.Repositories;
using MP.Core.Application.Wrapper;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MP.Core.Application.Features.Queries.CreditCardQueries
{
    public class CreditCardListQuery : IRequest<ApiResponse>
    {
        public class CreditCardListQueryHandler : IRequestHandler<CreditCardListQuery, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CreditCardListQueryHandler(ICreditCardRepository creditCardRepository, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _creditCardRepository = creditCardRepository;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CreditCardListQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    List<CreditCard> getAllResponse = await _creditCardRepository.GetAll();
                    if (!getAllResponse.Any())
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardListQuery DataNotFound", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardListQuery DataNotFound");
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Information, "CreditCardListQuery Success");
                        response = ApiResponse.SuccessResponse(getAllResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CreditCardListQuery Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CreditCardListQuery Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}