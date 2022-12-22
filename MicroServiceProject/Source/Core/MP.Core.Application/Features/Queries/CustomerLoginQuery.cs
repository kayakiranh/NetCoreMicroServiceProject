using MP.Core.Domain.Entities;
using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MP.Core.Application.Repositories;
using System.Threading;
using MP.Core.Domain.Enums;
using MP.Core.Application.Wrapper;


namespace MP.Core.Application.Features.Queries
{
    public class CustomerLoginQuery : IRequest<ApiResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class CustomerLoginQueryHandler : IRequestHandler<CustomerLoginQuery, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;
            private readonly ILoggerRepository _logger;

            public CustomerLoginQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILoggerRepository logger)
            {
                _customerRepository = customerRepository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CustomerLoginQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer loginResponse = await _customerRepository.Login(request.Email, request.Password);
                    if (loginResponse == null)
                    {
                        _logger.Insert(LogTypes.Error, "CustomerLoginQuery Error", null, request);
                        response = ApiResponse.ErrorResponse("CustomerLoginQuery BadRequest");
                    }

                    if (loginResponse.Id > 0)
                    {
                        _logger.Insert(LogTypes.Information, "CustomerLoginQuery Success");
                        response = ApiResponse.SuccessResponse(loginResponse);
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Error, "CustomerLoginQuery Error", null, request);
                        response = ApiResponse.ErrorResponse("CustomerLoginQuery Error");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerLoginQuery Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerLoginQuery Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}