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
    public class CreditCardListByFinancialScoreQuery : IRequest<ApiResponse>
    {
        public double MinScore { get; set; }
        public double MaxScore { get; set; }

        public class CreditCardListByFinancialScoreQueryHandler : IRequestHandler<CreditCardListByFinancialScoreQuery, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly ILoggerRepository _logger;

            public CreditCardListByFinancialScoreQueryHandler(ICreditCardRepository creditCardRepository, ILoggerRepository logger)
            {
                _creditCardRepository = creditCardRepository;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CreditCardListByFinancialScoreQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    List<CreditCard> listByFinancialScoreResponse = await _creditCardRepository.ListByFinancialScore(request.MinScore, request.MaxScore);
                    if (!listByFinancialScoreResponse.Any())
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardListByFinancialScoreQuery DataNotFound", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardListByFinancialScoreQuery DataNotFound");
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Information, "CreditCardListByFinancialScoreQuery Success");
                        response = ApiResponse.SuccessResponse(listByFinancialScoreResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CreditCardListByFinancialScoreQuery Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CreditCardListByFinancialScoreQuery Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}