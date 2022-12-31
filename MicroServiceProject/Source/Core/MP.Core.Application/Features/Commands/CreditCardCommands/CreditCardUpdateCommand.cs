﻿using AutoMapper;
using MediatR;
using MP.Core.Application.DataTransferObjects;
using MP.Core.Application.Repositories;
using MP.Core.Application.Wrapper;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MP.Core.Application.Features.Commands.CreditCardCommands
{
    public class CreditCardUpdateCommand : IRequest<ApiResponse>
    {
        public CreditCardDto CreditCardDto { get; set; }

        public class CreditCardUpdateCommandHandler : IRequestHandler<CreditCardUpdateCommand, ApiResponse>
        {
            private readonly ICreditCardRepository _creditCardRepository;
            private readonly IMapper _mapper;
            private readonly ILoggerRepository _logger;
            private readonly ICacheRepository _cacheRepository;

            public CreditCardUpdateCommandHandler(ICreditCardRepository creditCardRepository, IMapper mapper, ILoggerRepository logger, ICacheRepository cacheRepository)
            {
                _creditCardRepository = creditCardRepository;
                _mapper = mapper;
                _logger = logger;
                _cacheRepository = cacheRepository;
            }

            public async Task<ApiResponse> Handle(CreditCardUpdateCommand request, CancellationToken cancellationToken)
            {
                ApiResponse response = new ApiResponse();
                try
                {
                    CreditCard CreditCard = _mapper.Map<CreditCard>(request.CreditCardDto);
                    CreditCard UpdateResponse = await _creditCardRepository.Update(CreditCard);

                    if (UpdateResponse.Id < 1)
                    {
                        _logger.Insert(LogTypes.Error, "CreditCardUpdateCommand Error", null, request);
                        response = ApiResponse.ErrorResponse("CreditCardUpdateCommand Error");
                    }
                    else
                    {
                        _cacheRepository.SetData(UpdateResponse.Name, UpdateResponse);
                        _logger.Insert(LogTypes.Information, "CreditCardUpdateCommand Success");
                        response = ApiResponse.SuccessResponse(UpdateResponse);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.Insert(LogTypes.Information, "CreditCardUpdateCommand Cancelled", ex, request, cancellationToken);
                    response = ApiResponse.ErrorResponse(ex);
                }
                catch (Exception ex)
                {
                    _logger.Insert(LogTypes.Critical, "CreditCardUpdateCommand Catch", ex, request);
                    response = ApiResponse.ErrorResponse(ex);
                }

                return response;
            }
        }
    }
}