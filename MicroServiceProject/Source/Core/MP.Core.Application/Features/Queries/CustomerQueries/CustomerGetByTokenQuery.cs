using MP.Core.Domain.Entities;
using System;
using System.Threading.Tasks;
using MediatR;
using MP.Core.Application.Repositories;
using System.Threading;
using MP.Core.Domain.Enums;
using MP.Core.Application.Wrapper;

namespace MP.Core.Application.Features.Queries.CustomerQueries
{
    public class CustomerGetByTokenQuery : IRequest<ApiResponse>
    {
        public string Token { get; set; }

        public class CustomerGetByTokenQueryHandler : IRequestHandler<CustomerGetByTokenQuery, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly ILoggerRepository _logger;

            public CustomerGetByTokenQueryHandler(ICustomerRepository customerRepository, ILoggerRepository logger)
            {
                _customerRepository = customerRepository;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CustomerGetByTokenQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer getByTokenResponse = await _customerRepository.GetByToken(request.Token);
                    if (getByTokenResponse.Id == 0)
                    {
                        _logger.Insert(LogTypes.Error, "CustomerGetByTokenQuery DataNotFound", null, request);
                        response = ApiResponse.ErrorResponse("CustomerGetByTokenQuery DataNotFound");
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Information, "CustomerGetByTokenQuery Success");
                        response = ApiResponse.SuccessResponse(getByTokenResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CustomerGetByTokenQuery Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerGetByTokenQuery Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}