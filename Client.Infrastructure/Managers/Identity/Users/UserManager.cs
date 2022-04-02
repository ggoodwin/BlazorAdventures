using System.Net.Http.Json;
using Application.Requests.Identity;
using Application.Responses.Identity;
using Client.Infrastructure.Configurations;
using Client.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Identity.Users
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UrlConfiguration> _config;

        public UserManager(HttpClient httpClient, IOptions<UrlConfiguration> config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IResult<List<UserResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.UserEndpoints.GetAll}");
            return await response.ToResult<List<UserResponse>>();
        }

        public async Task<IResult<UserResponse>> GetAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.UserEndpoints.Get(userId)}");
            return await response.ToResult<UserResponse>();
        }

        public async Task<IResult> RegisterUserAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.UserEndpoints.Register}", request);
            return await response.ToResult();
        }

        public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.UserEndpoints.ToggleUserStatus}", request);
            return await response.ToResult();
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.UserEndpoints.GetUserRoles(userId)}");
            return await response.ToResult<UserRolesResponse>();
        }

        public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_config.Value.Api}/{Routes.UserEndpoints.GetUserRoles(request.UserId)}", request);
            return await response.ToResult<UserRolesResponse>();
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.UserEndpoints.ForgotPassword}", model);
            return await response.ToResult();
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.UserEndpoints.ResetPassword}", request);
            return await response.ToResult();
        }

        public async Task<string> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? $"{_config.Value.Api}/{Routes.UserEndpoints.Export}"
                : $"{_config.Value.Api}/{Routes.UserEndpoints.ExportFiltered(searchString)}");
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
    }
}
