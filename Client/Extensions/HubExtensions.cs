using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Constants.Application;

namespace Client.Extensions
{
    public static class HubExtensions
    {
        public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager, ILocalStorageService _localStorage, IConfiguration _config)
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(navigationManager.ToAbsoluteUri($"{_config["Urls:Api"]}{ApplicationConstants.SignalR.HubUrl}"), options => {
                        options.AccessTokenProvider = async () => (await _localStorage.GetItemAsync<string>("authToken"));
                    })
                    .WithAutomaticReconnect()
                    .Build();
            }
            return hubConnection;
        }
        public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager)
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
                    .Build();
            }
            return hubConnection;
        }
    }
}
