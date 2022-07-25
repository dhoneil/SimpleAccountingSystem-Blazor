using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class UomPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] IUomService UomService { get; set; } = null!;
        public List<Uom> Uoms { get; set; } = new();
        public Uom CurrentUom { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Uoms = await UomService.GetUoms();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "Uommodal", action);
        }

        public async Task AddNewLocation()
        {
            CurrentUom = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (CurrentUom.UomId > 0)
            {
                await UomService.UpdateItem(CurrentUom);
            }
            else
            {
                await UomService.CreateItem(CurrentUom);
            }

            CurrentUom = new();
            await LoadData();
            await ModalAction("hide");
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            var item = (Uom)args.Item;
            var locationid = item.UomId;
            CurrentUom = await UomService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
