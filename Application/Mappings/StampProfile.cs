using Application.Features.Stamps.Commands.AddEdit;
using Application.Features.Stamps.Queries.GetAll;
using Application.Features.Stamps.Queries.GetById;
using AutoMapper;
using Domain.Entities.TimeStamp;

namespace Application.Mappings
{
    public class StampProfile : Profile
    {
        public StampProfile()
        {
            CreateMap<AddEditStampCommand, Stamp>().ReverseMap();
            CreateMap<GetStampByIdResponse, Stamp>().ReverseMap();
            CreateMap<GetAllStampsResponse, Stamp>().ReverseMap();
        }
    }
}
