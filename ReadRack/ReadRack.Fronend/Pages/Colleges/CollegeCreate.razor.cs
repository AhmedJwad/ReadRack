using BlazorBootstrap;
using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using ReadRack.Fronend.Repositories;
using ReadRack.Shared.Entites;
using System.Diagnostics.Metrics;

namespace ReadRack.Fronend.Pages.Colleges
{
    public partial class CollegeCreate
    {
        private College college = new();
       
        private bool loading;
        private string? imageUrl;
        // Alert Properties
        private string? alertMessage;
        private string alertType = "alert-info"; // Default alert type (Bootstrap class)
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;           
        [Inject] private IRepository repository { get; set; } = null!;
       

        [Parameter, SupplyParameterFromQuery] public bool IsAdmin { get; set; }

       
        private void ImageSelected(string imagenBase64)
        {
           college.Photo = imagenBase64;
            imageUrl = null;
        }

      
        private async Task CreteCollegeAsync()
        {
          
            loading = true;

            var responseHttp = await repository.PostAsync<College>("/api/Colleges/full",college);
            loading = false;

            if (responseHttp.Error)
            {
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
