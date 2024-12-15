using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using ReadRack.Fronend.Repositories;
using ReadRack.Shared.Entites;

namespace ReadRack.Fronend.Pages.Colleges
{
    public partial class CollegeDetails
    {
        private College? college;
        private bool loading = true;

        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Parameter] public int collegeId { get; set; }

        protected async override Task OnInitializedAsync()
        {
           await LoadCollegeAsync();
        }

        private async Task LoadCollegeAsync()
        {
            loading = true;
            var httpActionResponse = await repository.GetAsync<College>($"api/Colleges/{collegeId}");
           
            if ((httpActionResponse.Error))
            {
                loading = false;
                var message=await httpActionResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message ,SweetAlertIcon.Error);
                return;
            }
            college = httpActionResponse.Response;
            loading = false;
        }
    }
}
