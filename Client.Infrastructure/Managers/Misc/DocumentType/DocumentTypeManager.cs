using System.Net.Http.Json;
using Application.Features.DocumentTypes.Commands.AddEdit;
using Application.Features.DocumentTypes.Queries.GetAll;
using Client.Infrastructure.Configurations;
using Client.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Misc.DocumentType
{
    public class DocumentTypeManager : IDocumentTypeManager
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UrlConfiguration> _config;

        public DocumentTypeManager(HttpClient httpClient, IOptions<UrlConfiguration> config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? $"{_config.Value.Api}/{Routes.DocumentTypesEndpoints.Export}"
                : $"{_config.Value.Api}/{Routes.DocumentTypesEndpoints.ExportFiltered(searchString)}");
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_config.Value.Api}/{Routes.DocumentTypesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllDocumentTypesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.DocumentTypesEndpoints.GetAll}");
            return await response.ToResult<List<GetAllDocumentTypesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.DocumentTypesEndpoints.Save}", request);
            return await response.ToResult<int>();
        }
    }
}
