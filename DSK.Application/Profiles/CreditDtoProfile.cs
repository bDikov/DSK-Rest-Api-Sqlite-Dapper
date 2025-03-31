using AutoMapper;
using DSK.Application.Models.DTOs;
using DSK.Domain.Entities;

namespace DSK.Application.Profiles;

public class CreditDTOProfile : Profile
{
    public CreditDTOProfile()
    {
        CreateMap<Credit, CreditDto>();
    }
}
