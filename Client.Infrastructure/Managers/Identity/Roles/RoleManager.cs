using System.Net.Http.Json;
using Application.Requests.Identity;
using Application.Responses.Identity;
using Client.Infrastructure.Configurations;
using Client.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Identity.Roles
{
    public class RoleManager : IRoleManager
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UrlConfiguration> _config;

        public RoleManager(HttpClient httpClient, IOptions<UrlConfiguration> config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IResult<string>> DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"{_config.Value.Api}/{Routes.RolesEndpoints.Delete}/{id}");
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<RoleResponse>>> GetRolesAsync()
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.RolesEndpoints.GetAll}");
            return await response.ToResult<List<RoleResponse>>();
        }

        public async Task<IResult<string>> SaveAsync(RoleRequest role)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.RolesEndpoints.Save}", role);
            return await response.ToResult<string>();
        }

        public async Task<IResult<PermissionResponse>> GetPermissionsAsync(string roleId)
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.RolesEndpoints.GetPermissions + roleId}");
            return await response.ToResult<PermissionResponse>();
        }

        public async Task<IResult<string>> UpdatePermissionsAsync(PermissionRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_config.Value.Api}/{Routes.RolesEndpoints.UpdatePermissions}", request);
            return await response.ToResult<string>();
        }
    }
}
