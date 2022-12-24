using MP.Core.Domain.Entities;
using System;
using System.Threading.Tasks;
using MediatR;
using MP.Core.Application.Repositories;
using System.Threading;
using MP.Core.Domain.Enums;
using MP.Core.Application.Wrapper;

namespace MP.Core.Application.Features.Commands.CreditCardCommands
{
    public class CreditCardRemoveCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }

        public class CreditCardRemoveCommandHandler : IRequestHandler<CreditCardRemoveCommand, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly ILoggerRepository _logger;

            public CreditCardRemoveCommandHandler(ICreditCardRepository creditCardRepository, ILoggerRepository logger)
            {
                _creditCardRepository = creditCardRepository;
                _logger = logger;
            }

            public async Task<ApiResponse> Handle(CreditCardRemoveCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    CreditCard getByIdResponse = await _creditCardRepository.GetById(request.Id);
                    if (getByIdResponse.Id == 0)
                    {
                        _creditCardRepository.Remove(request.Id);

                        CreditCard check = await _creditCardRepository.GetById(request.Id);
                        if (check.Id != 0)
                        {                            
                            _logger.Insert(LogTypes.Error, "CreditCardRemoveCommand Error", null, request);
                            response = ApiResponse.ErrorResponse("CreditCardRemoveCommand Error");
                        }
                        else
                        {
                            _logger.Insert(LogTypes.Information, "CreditCardRemoveCommand Success");
                            response = ApiResponse.SuccessResponse(getByIdResponse);
                        }
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