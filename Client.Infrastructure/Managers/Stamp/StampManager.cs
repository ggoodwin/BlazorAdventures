using System.Net.Http.Json;
using Application.Features.Stamps.Commands.AddEdit;
using Application.Features.Stamps.Commands.Import;
using Application.Features.Stamps.Queries.GetAll;
using Client.Infrastructure.Configurations;
using Client.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Stamp
{
    public class StampManager : IStampManager
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UrlConfiguration> _config;

        public StampManager(HttpClient httpClient, IOptions<UrlConfiguration> config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? $"{_config.Value.Api}/{Routes.StampEndpoints.Export}"
                : _config.Value.Api + "/" + Routes.StampEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_config.Value.Api}/{Routes.StampEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllStampsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.StampEndpoints.GetAll}");
            return await response.ToResult<List<GetAllStampsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditStampCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.StampEndpoints.Save}", request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ImportAsync(ImportStampCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.StampEndpoints.Import}", request);
            return await response.ToResult<int>();
        }
    }
}
