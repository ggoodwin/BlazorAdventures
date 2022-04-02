using Application.Responses.Identity;
using AutoMapper;
using Infrastructure.Models.Identity;

namespace Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, BlazorAdventuresRole>().ReverseMap();
        }
    }
}
