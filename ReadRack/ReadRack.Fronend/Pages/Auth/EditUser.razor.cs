using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using ReadRack.Fronend.Repositories;
using ReadRack.Fronend.Services;
using ReadRack.Shared.DTOs;
using ReadRack.Shared.Entites;
using System.Diagnostics.Metrics;
using System.Net;

namespace ReadRack.Fronend.Pages.Auth
{
    public partial class EditUser
    {
        private User? user;
        private List<College>? colleges;
       
        private string? imageUrl;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private ILoginService loginService { get; set; } = null!;
        [CascadingParameter] IModalService Modal { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserAsyc();
            await LoadCollegesAsync();
           
            if (!string.IsNullOrEmpty(user!.Photo))
            {
                //imageUrl = user.Photo;
                var imageName = Path.GetFileName(user.Photo);
                imageUrl = $"https://localhost:7186/images/users/{imageName}";
                user.Photo = null;
            }

        }



        private async Task LoadUserAsyc()
        {
            var responseHttp = await repository.GetAsync<User>($"/api/accounts");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/");
                    return;
                }
                var messageError = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }
            user = responseHttp.Response;
        }

        private void ImageSelected(string imagenBase64)
        {
            user!.Photo = imagenBase64;
            imageUrl = null;
        }

        private async Task CountryChangedAsync(ChangeEventArgs e)
        {
            var selectedCountry = Convert.ToInt32(e.Value);          
            user!.collegeId = 0;          

        }


       


        private async Task LoadCollegesAsync()
        {
            var responseHttp = await repository.GetAsync<List<College>>("/api/colleges/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            colleges = responseHttp.Response;
        }
        
        private async Task SaveUserAsync()
        {
            var responseHttp = await repository.PutAsync<User, TokenDTO>("/api/accounts", user!);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            await loginService.LoginAsync(responseHttp.Response!.Token);
            navigationManager.NavigateTo("/");
        }
        private void ShowModal()
        {
            Modal.Show<ChangePassword>();
        }
    }
}
