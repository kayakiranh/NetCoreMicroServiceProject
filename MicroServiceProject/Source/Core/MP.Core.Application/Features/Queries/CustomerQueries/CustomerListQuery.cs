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

namespace MP.Core.Application.Features.Queries.CustomerQueries
{
    public class CustomerListQuery : IRequest<ApiResponse>
    {
        public class CustomerListQueryHandler : IRequestHandler<CustomerListQuery, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CustomerListQueryHandler(ICustomerRepository customerRepository, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _customerRepository = customerRepository;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CustomerListQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    List<Customer> getAllResponse = _cacheRepository.GetAll<Customer>(); //await _customerRepository.GetAll();
                    if (!getAllResponse.Any())
                    {
                        _logger.Insert(LogTypes.Error, "CustomerListQuery DataNotFound", null, request);
                        response = ApiResponse.ErrorResponse("CustomerListQuery DataNotFound");
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Information, "CustomerListQuery Success");
                        response = ApiResponse.SuccessResponse(getAllResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CustomerListQuery Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerListQuery Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}