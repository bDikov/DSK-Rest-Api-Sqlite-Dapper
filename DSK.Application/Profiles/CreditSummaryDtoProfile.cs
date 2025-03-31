using AutoMapper;
using DSK.Application.Models.DTOs;
using DSK.Domain.ValueObject;

namespace DSK.Application.Profiles;

public class CreditSummaryDtoProfile : Profile
{
    public CreditSummaryDtoProfile()
    {
        CreateMap<CreditSummary, CreditSummaryDto>();
    }
}
