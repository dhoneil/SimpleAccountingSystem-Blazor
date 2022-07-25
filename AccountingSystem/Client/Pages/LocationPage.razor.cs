using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class LocationPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] ILocationService LocationService { get; set; } = null!;
        public List<Location> Locations { get; set; } = new();
        public Location CurrentLocation { get; set; } = new();
        public bool IsValidSubmit { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Locations = await LocationService.GetLocations();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "locationmodal", action);
        }

        public async Task AddNewLocation()
        {
            IsValidSubmit = true;
            CurrentLocation = new();
            await ModalAction("show");
        }

        public async Task SaveLocation()
        {
            if (String.IsNullOrEmpty(CurrentLocation.LocationName))
            {
                IsValidSubmit = false;
            }
            else if (Locations.Where(s => s.LocationName == CurrentLocation.LocationName).ToList().Count() > 0)
            {
                IsValidSubmit = false;
            }
            else
            {
                IsValidSubmit = true;
            }

            if (IsValidSubmit)
            {
                if (CurrentLocation.LocationId > 0)
                {
                    await LocationService.UpdateLocation(CurrentLocation);
                }
                else
                {
                    await LocationService.CreateLocation(CurrentLocation);
                }

                CurrentLocation = new();
                await LoadData();
                await ModalAction("hide");
            }
        }

        public async Task EditLocation(GridCommandEventArgs args)
        {
            var item = (Location)args.Item;
            var locationid = item.LocationId;
            CurrentLocation = await LocationService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
