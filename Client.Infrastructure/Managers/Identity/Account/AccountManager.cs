using System.Net.Http.Json;
using Application.Requests.Identity;
using Client.Infrastructure.Configurations;
using Client.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Identity.Account
{
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UrlConfiguration> _config;

        public AccountManager(HttpClient httpClient, IOptions<UrlConfiguration> config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_config.Value.Api}/{Routes.AccountEndpoints.ChangePassword}", model);
            return await response.ToResult();
        }

        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_config.Value.Api}/{Routes.AccountEndpoints.UpdateProfile}", model);
            return await response.ToResult();
        }

        public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.AccountEndpoints.GetProfilePicture(userId)}");
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.AccountEndpoints.UpdateProfilePicture(userId)}", request);
            return await response.ToResult<string>();
        }
    }
}
