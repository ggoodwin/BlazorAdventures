using Application.Responses.Audit;
using AutoMapper;
using Infrastructure.Models.Audit;

namespace Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}
