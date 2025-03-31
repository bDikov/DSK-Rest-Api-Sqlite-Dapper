using AutoMapper;
using DSK.Domain.Entities;
using DSK.Infrastructure.Database.Models;

namespace DSK.Infrastructure.Profiles;

public class CreditProfile : Profile
{
    public CreditProfile()
    {
        CreateMap<CreditDbModel, Credit>()
            .ConstructUsing((source, context) => new Credit(
                number: source.Number,
                name: source.Name,
                amount: source.Amount,
                requestDate: source.RequestDate,
                status: source.Status.ToString(),
                invoices: context.Mapper.Map<List<Invoice>>(source.Invoices))
            );
    }
}
