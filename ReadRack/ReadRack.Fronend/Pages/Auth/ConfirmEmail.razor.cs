using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using ReadRack.Fronend.Repositories;

namespace ReadRack.Fronend.Pages.Auth
{
    public partial class ConfirmEmail
    {
        private string? message;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;     
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string UserId { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Token { get; set; } = string.Empty;

        private async Task ConfirmAccountAsync()
        {
            var responseHttp = await repository.GetAsync($"/api/accounts/ConfirmEmail/?userId={UserId}&token={Token}");

            if (responseHttp.Error)
            {
                message = await responseHttp.GetErrorMessageAsync();
                NavigationManager.NavigateTo("/");
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await SweetAlertService.FireAsync("Success", "Thank you for confirming your email, you can now log in to the system.", SweetAlertIcon.Info);
            NavigationManager.NavigateTo("/users");
        }
    }
}
