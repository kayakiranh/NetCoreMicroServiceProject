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
    public class CreditCardListQuery : IRequest<ApiResponse>
    {
        public class CreditCardListQueryHandler : IRequestHandler<CreditCardListQuery, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly ILoggerRepository _logger;

            public CreditCardListQueryHandler(ICreditCardRepository creditCardRepository, ILoggerRepository logger)
            {
                _creditCardRepository = creditCardRepository;
                _logger = logger;
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