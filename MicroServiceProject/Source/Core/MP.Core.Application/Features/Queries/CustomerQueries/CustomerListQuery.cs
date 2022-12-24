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

namespace MP.Core.Application.Features.Queries.CustomerQueries
{
    public class CustomerListQuery : IRequest<ApiResponse>
    {
        public class CustomerListQueryHandler : IRequestHandler<CustomerListQuery, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly ILoggerRepository _logger;

            public CustomerListQueryHandler(ICustomerRepository customerRepository, ILoggerRepository logger)
            {
                _customerRepository = customerRepository;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CustomerListQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    List<Customer> getAllResponse = await _customerRepository.GetAll();
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