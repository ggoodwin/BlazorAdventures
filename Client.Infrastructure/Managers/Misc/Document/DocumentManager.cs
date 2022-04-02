using System.Net.Http.Json;
using Application.Features.Documents.Commands.AddEdit;
using Application.Features.Documents.Queries.GetAll;
using Application.Features.Documents.Queries.GetById;
using Application.Requests.Documents;
using Client.Infrastructure.Configurations;
using Client.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Misc.Document
{
    public class DocumentManager : IDocumentManager
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UrlConfiguration> _config;

        public DocumentManager(HttpClient httpClient, IOptions<UrlConfiguration> config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_config.Value.Api}/{Routes.DocumentsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request)
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.DocumentsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString)}");
            return await response.ToPaginatedResult<GetAllDocumentsResponse>();
        }

        public async Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request)
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.DocumentsEndpoints.GetById(request.Id)}");
            return await response.ToResult<GetDocumentByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDocumentCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.DocumentsEndpoints.Save}", request);
            return await response.ToResult<int>();
        }
    }
}
