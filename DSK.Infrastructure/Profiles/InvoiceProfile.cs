using AutoMapper;
using DSK.Domain.Entities;
using DSK.Infrastructure.Database.Models;

namespace DSK.Infrastructure.Profiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<InvoiceDbModel, Invoice>()
                .ConstructUsing((source, context) => new Invoice(
                    number: source.Number,
                    amount: source.Amount)
                );
        }
    }
}