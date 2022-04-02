using Application.Responses.Audit;
using Client.Infrastructure.Configurations;
using Client.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Audit
{
    public class AuditManager : IAuditManager
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UrlConfiguration> _config;

        public AuditManager(HttpClient httpClient, IOptions<UrlConfiguration> config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync()
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.AuditEndpoints.GetCurrentUserTrails}");
            var data = await response.ToResult<IEnumerable<AuditResponse>>();
            return data;
        }

        public async Task<IResult<string>> DownloadFileAsync(string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false)
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? $"{_config.Value.Api}/{Routes.AuditEndpoints.DownloadFile}"
                : _config.Value.Api + "/" + Routes.AuditEndpoints.DownloadFileFiltered(searchString, searchInOldValues, searchInNewValues));
            return await response.ToResult<string>();
        }
    }
}
