using AutoMapper;
using DSK.Application.Models.DTOs;
using DSK.Domain.Entities;

namespace DSK.Application.Profiles;

public class InvoiceDtoProfile : Profile
{
    public InvoiceDtoProfile()
    {
        CreateMap<Invoice, InvoiceDto>();
    }
}