using MediatR;
using MP.Core.Application.Repositories;
using MP.Core.Application.Wrapper;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MP.Core.Application.Features.Commands.CustomerCommands
{
    public class CustomerLoginCommand : IRequest<ApiResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class CustomerLoginCommandHandler : IRequestHandler<CustomerLoginCommand, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly ILoggerRepository _logger;

            public CustomerLoginCommandHandler(ICustomerRepository customerRepository, ILoggerRepository logger)
            {
                _customerRepository = customerRepository;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CustomerLoginCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer loginResponse = await _customerRepository.Login(request.Email, request.Password);
                    if (loginResponse.Id < 1)
                    {
                        _logger.Insert(LogTypes.Error, "CustomerLoginCommand Error", null, request);
                        response = ApiResponse.ErrorResponse("CustomerLoginCommand Error");
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Information, "CustomerLoginCommand Success");
                        response = ApiResponse.SuccessResponse(loginResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CustomerLoginCommand Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerLoginCommand Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}