using Application.Requests.Identity;
using Application.Responses.Identity;
using AutoMapper;
using Infrastructure.Models.Identity;

namespace Infrastructure.Mappings
{
    public class RoleClaimProfile : Profile
    {
        public RoleClaimProfile()
        {
            CreateMap<RoleClaimResponse, BlazorAdventuresRoleClaim>()
                .ForMember(nameof(BlazorAdventuresRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(BlazorAdventuresRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, BlazorAdventuresRoleClaim>()
                .ForMember(nameof(BlazorAdventuresRoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(BlazorAdventuresRoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();
        }
    }
}
