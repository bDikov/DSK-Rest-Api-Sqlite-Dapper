using AutoMapper;
using DSK.Api.Contracts.Responses;
using DSK.Application.Models.DTOs;

namespace DSK.Api.Profiles;

public class CreditSummaryResponseProfile : Profile
{
    public CreditSummaryResponseProfile()
    {
        CreateMap<CreditSummaryDto, CreditSummaryResponse>();
    }
}
