using AutoMapper;
using MP.Core.Application.DataTransferObjects;
using MP.Core.Domain.Entities;
using System;

namespace MP.Core.Application.Mapping
{
    /// <summary>
    /// Automapper, mapping class
    /// ReverseMap for mapping 2 sides.
    /// </summary>
    [Serializable]
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<CreditCard, CreditCardDto>().ReverseMap(); //Credit card mapping, entity <> dto
            CreateMap<Customer, CustomerDto>().ReverseMap(); //Customer mapping, entity <> dto
        }
    }
}