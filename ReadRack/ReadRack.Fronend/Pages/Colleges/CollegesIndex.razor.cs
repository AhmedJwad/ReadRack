using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using ReadRack.Fronend.Repositories;
using ReadRack.Shared.Entites;
using System.Diagnostics.Metrics;

namespace ReadRack.Fronend.Pages.Colleges
{
    public partial class CollegesIndex
    {
        //public List<College>? Colleges { get; set; }
        private IEnumerable<College> colleges = default!;

        private HashSet<College> selectedColleges = new();

        private readonly int[] pageSizeOptions = { 10, 25, 50, 5, int.MaxValue };
        private int totalRecords = 0;
        private bool loading;
        private const string baseUrl = "api/Countries";
        private string infoFormat = "{first_item}-{last_item} of {all_items}";
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 10;
        [Inject] private IRepository repository { get; set; } = null!;
        AlertColor alertColor = AlertColor.Primary;
        IconName alertIconName = IconName.CheckCircleFill;
        private string  alertMessage { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;


        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            await LoadTotalRecords();
        }

        private async Task<bool> LoadTotalRecords()
        {
            loading = true;
            var url = $"api/Colleges/recordsNumber?Page=1&RecordsNumber={int.MaxValue}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Filter}";
            }
            var responseHttp = await repository.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                alertColor = AlertColor.Success;
                alertIconName = IconName.CheckCircleFill;
                alertMessage = message!;
                return false;
            }
            totalRecords = responseHttp.Response;
            loading = false;
            return true;
        }
        private async Task<GridDataProviderResult<College>> CollegeDataProvider(GridDataProviderRequest<College> request)
        {
            if (colleges is null) // pull employees only one time for client-side filtering, sorting, and paging
               colleges = await GetCollegesAsync();  // call a service or an API to pull the employees
          

            return await Task.FromResult(request.ApplyTo(colleges));
        }

        private async Task<IEnumerable<College>> GetCollegesAsync()
        {
            var responseHttp = await repository.GetAsync<List<College>>("api/Colleges/full");

            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                alertColor = AlertColor.Success;
                alertIconName = IconName.CheckCircleFill;
                alertMessage = message!;
                return Enumerable.Empty<College>();
            }

            return responseHttp.Response ?? Enumerable.Empty<College>();
        }
        private Task OnSelectedItemsChanged(HashSet<College> colleges)
        {
            selectedColleges = colleges is not null && colleges.Any() ? colleges : new();
            return Task.CompletedTask;
        }
    }
}
