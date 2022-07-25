using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class BrandPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] IBrandService BrandService { get; set; } = null!;
        public List<Brand> Brands { get; set; } = new();
        public Brand CurrentBrand { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Brands = await BrandService.GetBrands();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "Brandmodal", action);
        }

        public async Task AddNewLocation()
        {
            CurrentBrand = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (CurrentBrand.BrandId > 0)
            {
                await BrandService.UpdateItem(CurrentBrand);
            }
            else
            {
                await BrandService.CreateItem(CurrentBrand);
            }

            CurrentBrand = new();
            await LoadData();
            await ModalAction("hide");
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            var item = (Brand)args.Item;
            var locationid = item.BrandId;
            CurrentBrand = await BrandService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
