using MediatR;
using MP.Core.Application.Repositories;
using MP.Core.Application.Wrapper;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MP.Core.Application.Features.Commands.CreditCardCommands
{
    public class CreditCardRemoveCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }

        public class CreditCardRemoveCommandHandler : IRequestHandler<CreditCardRemoveCommand, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CreditCardRemoveCommandHandler(ICreditCardRepository creditCardRepository, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _creditCardRepository = creditCardRepository;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CreditCardRemoveCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    CreditCard getByIdResponse = await _creditCardRepository.GetById(request.Id);
                    if (getByIdResponse.Id == 0)
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardRemoveCommand DataNotFound", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardRemoveCommand DataNotFound");
                    }

                    _creditCardRepository.Remove(request.Id);

                    CreditCard check = await _creditCardRepository.GetById(request.Id);
                    if (check.Id != 0)
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardRemoveCommand Error", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardRemoveCommand Error");
                    }
                    else
                    {
                        _cacheRepository.RemoveData(check, check.Name);
                        _logger.Insert(LogTypes.Information, "CreditCardRemoveCommand Success");
                        response = ApiResponse.SuccessResponse(getByIdResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CreditCardRemoveCommand Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CreditCardRemoveCommand Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}