﻿using MP.Core.Domain.Entities;
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
    public class CreditCardListByTypeQuery : IRequest<ApiResponse>
    {
        public CreditCardTypes CreditCardType { get; set; }

        public class CreditCardListByTypeQueryHandler : IRequestHandler<CreditCardListByTypeQuery, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly ILoggerRepository _logger;

            public CreditCardListByTypeQueryHandler(ICreditCardRepository creditCardRepository, ILoggerRepository logger)
            {
                _creditCardRepository = creditCardRepository;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CreditCardListByTypeQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    List<CreditCard> listByTypeResponse = await _creditCardRepository.ListByType(request.CreditCardType);
                    if (!listByTypeResponse.Any())
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardListByTypeQuery DataNotFound", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardListByTypeQuery DataNotFound");
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Information, "CreditCardListByTypeQuery Success");
                        response = ApiResponse.SuccessResponse(listByTypeResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CreditCardListByTypeQuery Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CreditCardListByTypeQuery Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}