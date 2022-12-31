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
    public class CustomerGetByIdentityNumberQuery : IRequest<ApiResponse>
    {
        public string IdentityNumber { get; set; }

        public class CustomerGetByIdentityNumberQueryHandler : IRequestHandler<CustomerGetByIdentityNumberQuery, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CustomerGetByIdentityNumberQueryHandler(ICustomerRepository customerRepository, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _customerRepository = customerRepository;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CustomerGetByIdentityNumberQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer getByIdentityNumberResponse = _cacheRepository.GetData<Customer>(request.IdentityNumber);
                    if (getByIdentityNumberResponse.Id == 0)
                    {
                        _logger.Insert(LogTypes.Error, "CustomerGetByIdentityNumberQuery Cache DataNotFound", null, request);

                        getByIdentityNumberResponse = await _customerRepository.GetByIdentityNumber(request.IdentityNumber);
                        if (getByIdentityNumberResponse.Id == 0)
                        {
                            _logger.Insert(LogTypes.Error, "CustomerGetByIdentityNumberQuery DataNotFound", null, request);
                            response = ApiResponse.ErrorResponse("CustomerGetByIdentityNumberQuery DataNotFound");
                        }
                        else
                        {
                            _cacheRepository.SetData<Customer>(getByIdentityNumberResponse.IdentityNumber, getByIdentityNumberResponse);
                            _logger.Insert(LogTypes.Information, "CustomerGetByIdentityNumberQuery Success");
                            response = ApiResponse.SuccessResponse(getByIdentityNumberResponse);
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CustomerGetByIdentityNumberQuery Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerGetByIdentityNumberQuery Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}