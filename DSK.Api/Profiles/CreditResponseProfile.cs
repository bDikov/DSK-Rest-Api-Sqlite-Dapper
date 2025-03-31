using AutoMapper;
using DSK.Api.Contracts.Responses;
using DSK.Application.Models.DTOs;

namespace DSK.Api.Profiles;

public class CreditResponseProfile : Profile
{
    public CreditResponseProfile()
    {
        CreateMap<CreditDto, CreditResponse>();
    }
}
