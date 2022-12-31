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
    public class CustomerRemoveCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }

        public class CustomerRemoveCommandHandler : IRequestHandler<CustomerRemoveCommand, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CustomerRemoveCommandHandler(ICustomerRepository CustomerRepository, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _customerRepository = CustomerRepository;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CustomerRemoveCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer getByIdResponse = await _customerRepository.GetById(request.Id);
                    if (getByIdResponse.Id == 0)
                    {
                        _customerRepository.Remove(request.Id);

                        Customer check = await _customerRepository.GetById(request.Id);
                        if (check.Id != 0)
                        {
                            _logger.Insert(LogTypes.Error, "CustomerRemoveCommand Error", null, request);
                            response = ApiResponse.ErrorResponse("CustomerRemoveCommand Error");
                        }
                        else
                        {
                            _cacheRepository.RemoveData(check, check.IdentityNumber);
                            _logger.Insert(LogTypes.Information, "CustomerRemoveCommand Success");
                            response = ApiResponse.SuccessResponse(getByIdResponse);
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CustomerRemoveCommand Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerRemoveCommand Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}