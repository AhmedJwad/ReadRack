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
    public partial class CollegesIndex
    {
        private int currentPage = 1;
        private int totalPages;
        [Inject] private IRepository Reposotory { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 10;
        [CascadingParameter] IModalService Modal { get; set; } = default!;
        public List<College>? colleges { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }
        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }
        private async Task LoadAsync(int page=1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();

            }
        }

       
        private async Task<bool> LoadListAsync(int page)
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/Colleges?page={page}&recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }
            var responseHttp = await Reposotory.GetAsync<List<College>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            }
            colleges = responseHttp.Response;
            return true;
        }
        private async Task LoadPagesAsync()
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/Colleges/totalPages?recordsnumber={RecordsNumber}";

            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&Filter={Filter}";
            }
            var responseHttp = await Reposotory.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHttp.Response;
        }
        private void ValidateRecordsNumber(int recordsnumber)
        {
            if (recordsnumber == 0)
            {
                RecordsNumber = 10;
            }
        }
        private async Task DeleteAsync(College college)
        {
            var result = await SweetAlertService.FireAsync(
                new SweetAlertOptions
                {
                    Title = "Confirmation",
                    Text = $"Are you sure you want to delete the country:{college.Name}?",
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,

                });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm) { return; }
            var responseHttp = await Reposotory.DeleteAsync<College>($"api/Colleges/{college.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    navigationManager.NavigateTo("/colleges");
                }
                else
                {
                    var messerror = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", messerror, SweetAlertIcon.Error);
                }
                return;
            }
            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Log successfully deleted");
        }
        private async Task CleanFilterAsync()
        {
            Filter = string.Empty;
            await ApplyFilterAsync();
        }

        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }
        private async Task FilterCallBack(string filter)
        {
            Filter = filter;
            await ApplyFilterAsync();
            StateHasChanged();
        }
        private async Task SelectedRecordsNumberAsync(int recordsnumber)
        {
            RecordsNumber = recordsnumber;
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }
        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IModalReference modalReference;

            if (isEdit)
            {
                modalReference = Modal.Show<CollegeEdit>(string.Empty, new ModalParameters().Add("id", id));
            }
            else
            {
                // modalReference = Modal.Show<CollegeCreate>();
                navigationManager.NavigateTo("/colleges/create");
            }
            
        }
    }
}
