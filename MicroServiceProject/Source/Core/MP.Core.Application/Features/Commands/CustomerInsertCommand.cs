using MP.Core.Domain.Entities;
using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MP.Core.Application.Repositories;
using System.Threading;
using MP.Core.Application.DataTransferObjects;
using MP.Core.Domain.Enums;
using MP.Core.Application.Wrapper;

namespace MP.Core.Application.Features.Commands
{
    public class CustomerInsertCommand : IRequest<ApiResponse>
    {
        public CustomerDto CustomerDto { get; set; }

        public class CustomerInsertCommandHandler : IRequestHandler<CustomerInsertCommand, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;
            private readonly ILoggerRepository _logger;

            public CustomerInsertCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ILoggerRepository logger)
            {
                _customerRepository = customerRepository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CustomerInsertCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer customer = _mapper.Map<Customer>(request.CustomerDto);
                    Customer insertResponse = await _customerRepository.Insert(customer);

                    if (insertResponse.Id > 0)
                    {
                        _logger.Insert(LogTypes.Information, "CustomerInsertCommand Success");
                        response = ApiResponse.SuccessResponse(insertResponse);
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Error, "CustomerInsertCommand Error", null, request);
                        response = ApiResponse.ErrorResponse("CustomerInsertCommand Error");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerInsertCommand Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerInsertCommand Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}