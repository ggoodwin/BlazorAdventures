using System.Security.Claims;
using Application.Features.Stamps.Commands.AddEdit;
using Application.Features.Stamps.Commands.Import;
using Application.Features.Stamps.Queries.GetAll;
using Application.Requests;
using Client.Extensions;
using Client.Infrastructure.Managers.Stamp;
using Client.Shared.Components;
using Domain.Entities.TimeStamp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Constants.Application;
using Shared.Constants.Permission;
using Shared.Wrapper;

namespace Client.Pages.Stamps
{
    public partial class Stamps
    {
        [Inject] private IStampManager StampManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllStampsResponse> _stampList = new();
        private GetAllStampsResponse _stamps = new();
        private string _searchString = "";
        private string _nextTime = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateStamps = true;
        private bool _canEditStamps = false;
        private bool _canDeleteStamps = true;
        private bool _canExportStamps = true;
        private bool _canSearchStamps = false;
        private bool _canImportStamps = true;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateStamps = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stamps.Create)).Succeeded;
            _canEditStamps = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stamps.Edit)).Succeeded;
            _canDeleteStamps = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stamps.Delete)).Succeeded;
            _canExportStamps = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stamps.Export)).Succeeded;
            _canSearchStamps = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stamps.Search)).Succeeded;
            _canImportStamps = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stamps.Import)).Succeeded;

            await GetStampAsync();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage, _config);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetStampAsync()
        {
            var response = await StampManager.GetAllAsync();
            if (response.Succeeded)
            {
                _stampList = response.Data.OrderByDescending(x=>x.TheStamp).ToList();
                _nextTime = _stampList.FirstOrDefault().TheStamp.ToLocalTime().AddHours(3).ToShortTimeString();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = "Delete Content";
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Delete", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await StampManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task<IResult<int>> ImportExcel(UploadRequest uploadFile)
        {
            var request = new ImportStampCommand { UploadRequest = uploadFile };
            var result = await StampManager.ImportAsync(request);
            return result;
        }

        private async Task InvokeImportModal()
        {
            var parameters = new DialogParameters
            {
                { nameof(ImportExcelModal.ModelName), "Stamps" }
            };
            Func<UploadRequest, Task<IResult<int>>> importExcel = ImportExcel;
            parameters.Add(nameof(ImportExcelModal.OnSaved), importExcel);
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                DisableBackdropClick = true
            };
            var dialog = _dialogService.Show<ImportExcelModal>("Import", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task ExportToExcel()
        {
            var response = await StampManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Stamp).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? "Stamps exported"
                    : "Filtered Stamps exported", Severity.Success);
            }
            else
            {
                if (!response.Messages.Any()) _snackBar.Add("Download Failed with Generic Error", Severity.Error);
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task AddStamp()
        {
            var command = new AddEditStampCommand
            {
                Id = 0,
                TheStamp = DateTime.UtcNow
            };
            
            var response = await StampManager.SaveAsync(command);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }

            await Reset();
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _stamps = _stampList.FirstOrDefault(c => c.Id == id);
                if (_stamps != null)
                {
                    parameters.Add(nameof(AddEditStampModal.AddEditStampModel), new AddEditStampCommand
                    {
                        Id = _stamps.Id,
                        TheStamp = _stamps.TheStamp
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditStampModal>(id == 0 ? "Create" : "Edit", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _stamps = new GetAllStampsResponse();
            await GetStampAsync();
            
        }

        private bool Search(GetAllStampsResponse stamp)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (stamp.TheStamp.Equals(_searchString))
            {
                return true;
            }

            return false;// stamp.Client?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
