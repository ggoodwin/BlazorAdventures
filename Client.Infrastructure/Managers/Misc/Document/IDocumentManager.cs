using Application.Features.Documents.Commands.AddEdit;
using Application.Features.Documents.Queries.GetAll;
using Application.Features.Documents.Queries.GetById;
using Application.Requests.Documents;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
