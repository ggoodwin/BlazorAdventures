using Application.Features.Documents.Commands.AddEdit;
using Application.Features.Documents.Queries.GetById;
using AutoMapper;
using Domain.Entities.Misc;

namespace Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}
