using Application.Features.DocumentTypes.Commands.AddEdit;
using Application.Features.DocumentTypes.Queries.GetAll;
using Application.Features.DocumentTypes.Queries.GetById;
using AutoMapper;
using Domain.Entities.Misc;

namespace Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}
