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
    public class CustomerUpdateCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }

        public class CustomerUpdateCommandHandler : IRequestHandler<CustomerUpdateCommand, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly ILoggerRepository _logger;

            public CustomerUpdateCommandHandler(ICustomerRepository customerRepository, ILoggerRepository logger)
            {
                _customerRepository = customerRepository;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer customer = await _customerRepository.GetById(request.Id);
                    if (customer != null)
                    {
                        _logger.Insert(LogTypes.Error, "CustomerUpdateCommand Error", null, request);
                        response = ApiResponse.ErrorResponse("CustomerUpdateCommand Error");
                    }

                    Customer updateResponse = await _customerRepository.Update(customer);

                    if (updateResponse.Id > 0)
                    {
                        _logger.Insert(LogTypes.Information, "CustomerUpdateCommand Success");
                        response = ApiResponse.SuccessResponse(updateResponse);
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Error, "CustomerUpdateCommand Error", null, request);
                        response = ApiResponse.ErrorResponse("CustomerUpdateCommand Error");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CustomerUpdateCommand Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerUpdateCommand Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}