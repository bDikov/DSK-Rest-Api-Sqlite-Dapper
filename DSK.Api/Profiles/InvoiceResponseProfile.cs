using AutoMapper;
using DSK.Api.Contracts.Responses;
using DSK.Application.Models.DTOs;

namespace DSK.Api.Profiles;

public class InvoiceResponseProfile : Profile
{
    public InvoiceResponseProfile()
    {
        CreateMap<InvoiceDto, InvoiceResponse>();
    }
}