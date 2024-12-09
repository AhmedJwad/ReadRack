using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using ReadRack.Fronend.Repositories;
using ReadRack.Shared.Entites;
using System.Diagnostics.Metrics;
using System.Net;

namespace ReadRack.Fronend.Pages.Colleges
{
    public partial class CollegeEdit
    {
        private College? college;     
        private string? imageUrl;
        private string? alertMessage;
        private string alertType = "alert-info"; // Default alert type (Bootstrap class)
        [Inject] private NavigationManager navigationManager { get; set; } = null!;        
        [Inject] private IRepository repository { get; set; } = null!;  
       
        [EditorRequired, Parameter] public int Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
           await LoadAsync();
            if (!string.IsNullOrEmpty(college!.Photo))
            {
                //imageUrl = user.Photo;
                var imageName = Path.GetFileName(college.Photo);
                imageUrl = $"{navigationManager.BaseUri}images/colleges/{imageName}";
                college.Photo = null;
            }

        }
        private  async Task LoadAsync()
        {
            var responseHttp = await repository.GetAsync<College>($"api/Colleges/{Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    navigationManager.NavigateTo("/colleges");
                }
                else
                {
                    
                    alertType = "alert-danger"; // Error alert
                    alertMessage = await responseHttp.GetErrorMessageAsync();
                }
            }
            else
            {
                college = responseHttp.Response;
            }

        }


     
        private void ImageSelected(string imagenBase64)
        {
            college!.Photo = imagenBase64;
            imageUrl = null;
        }


        private async Task EditAsync()
        {
            var responseHttp = await repository.PutAsync("api/colleges/full", college);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                alertType = "alert-danger"; // Error alert
                alertMessage = await responseHttp.GetErrorMessageAsync();
                return;
            }

            alertType = "alert-success"; // Success alert
            alertMessage = "College added successfully.";
            navigationManager.NavigateTo("/colleges");
        }

    }
}
