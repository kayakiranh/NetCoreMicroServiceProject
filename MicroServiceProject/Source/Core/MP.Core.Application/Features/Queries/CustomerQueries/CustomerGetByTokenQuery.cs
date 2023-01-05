using MediatR;
using MP.Core.Application.Repositories;
using MP.Core.Application.Wrapper;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MP.Core.Application.Features.Queries.CustomerQueries
{
    public class CustomerGetByTokenQuery : IRequest<ApiResponse>
    {
        public string Token { get; set; }

        public class CustomerGetByTokenQueryHandler : IRequestHandler<CustomerGetByTokenQuery, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CustomerGetByTokenQueryHandler(ICustomerRepository customerRepository, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _customerRepository = customerRepository;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CustomerGetByTokenQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer getByTokenResponse = _cacheRepository.GetByValue<Customer>("Token", request.Token);
                    if (getByTokenResponse.Id == 0)
                    {
                        _logger.Insert(LogTypes.Error, "CustomerGetByTokenQuery Cache DataNotFound", null, request);
                        getByTokenResponse = await _customerRepository.GetByToken(request.Token);
                        if (getByTokenResponse.Id == 0)
                        {
                            _logger.Insert(LogTypes.Error, "CustomerGetByTokenQuery DataNotFound", null, request);
                            response = ApiResponse.ErrorResponse("CustomerGetByTokenQuery DataNotFound");
                        }
                        else
                        {
                            _cacheRepository.SetData<Customer>(getByTokenResponse.IdentityNumber, getByTokenResponse);
                            _logger.Insert(LogTypes.Error, "CustomerGetByTokenQuery Success", null, request);
                            response = ApiResponse.ErrorResponse("CustomerGetByTokenQuery Success");
                        }
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