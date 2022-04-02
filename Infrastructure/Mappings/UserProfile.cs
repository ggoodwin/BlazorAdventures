using Application.Responses.Identity;
using AutoMapper;
using Infrastructure.Models.Identity;

namespace Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, BlazorAdventuresUser>().ReverseMap();
            //CreateMap<ChatUserResponse, BlazorHeroUser>().ReverseMap()
            //    .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping
        }
    }
}
