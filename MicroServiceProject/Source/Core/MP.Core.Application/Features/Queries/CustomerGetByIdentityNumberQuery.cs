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
    public class CustomerGetByIdentityNumberQuery : IRequest<ApiResponse>
    {
        public string IdentityNumber { get; set; }

        public class CustomerGetByIdentityNumberQueryHandler : IRequestHandler<CustomerGetByIdentityNumberQuery, ApiResponse>
        {
            private readonly ICustomerRepository _customerRepository;
            private readonly IMapper _mapper;
            private readonly ILoggerRepository _logger;

            public CustomerGetByIdentityNumberQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILoggerRepository logger)
            {
                _customerRepository = customerRepository;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CustomerGetByIdentityNumberQuery request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    Customer getByIdentityNumberResponse = await _customerRepository.GetByIdentityNumber(request.IdentityNumber);
                    if (getByIdentityNumberResponse == null)
                    {
                        _logger.Insert(LogTypes.Error, "CustomerGetByIdentityNumberQuery Error", null, request);
                        response = ApiResponse.ErrorResponse("CustomerGetByIdentityNumberQuery BadRequest");
                    }

                    if (getByIdentityNumberResponse.Id > 0)
                    {
                        _logger.Insert(LogTypes.Information, "CustomerGetByIdentityNumberQuery Success");
                        response = ApiResponse.SuccessResponse(getByIdentityNumberResponse);
                    }
                    else
                    {
                        _logger.Insert(LogTypes.Error, "CustomerGetByIdentityNumberQuery Error", null, request);
                        response = ApiResponse.ErrorResponse("CustomerGetByIdentityNumberQuery Error");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Critical, "CustomerGetByIdentityNumberQuery Cancelled", ex, request, cancellationToken);
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