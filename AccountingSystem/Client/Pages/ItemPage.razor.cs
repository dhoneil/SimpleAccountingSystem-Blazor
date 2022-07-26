using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class ItemPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] IPartNumberService PartNumberService { get; set; } = null!;
        [Inject] IUomService UomService { get; set; } = null!;
        [Inject] IItemService ItemService { get; set; } = null!;
        public List<Item> Items { get; set; } = new();
        public List<PartNumber> PartNumbers { get; set; } = new();
        public List<Uom> Uoms { get; set; } = new();
        public Item CurrentItem { get; set; } = new();
        public bool IsValidSubmit { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Uoms = await UomService.GetUoms();
            PartNumbers = await PartNumberService.GetPartNumbers();
            Items = await ItemService.GetItems();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "Itemmodal", action);
        }

        public async Task AddNewLocation()
        {
            IsValidSubmit = true;
            CurrentItem = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (String.IsNullOrEmpty(CurrentItem.ItemCode))
            {
                IsValidSubmit = false;
            }
            else if (String.IsNullOrEmpty(CurrentItem.ItemDescription))
            {
                IsValidSubmit = false;
            }
            else if (Items.Where(s => s.ItemCode == CurrentItem.ItemCode && s.ItemDescription == CurrentItem.ItemDescription).ToList().Count() > 0)
            {
                IsValidSubmit = false;
            }
            else
            {
                IsValidSubmit = true;
            }


            if (IsValidSubmit)
            {
                if (CurrentItem.ItemId > 0)
                {
                    await ItemService.UpdateItem(CurrentItem);
                }
                else
                {
                    await ItemService.CreateItem(CurrentItem);
                }

                CurrentItem = new();
                await LoadData();
                await ModalAction("hide");
            }
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            var item = (Item)args.Item;
            var locationid = item.ItemId;
            CurrentItem = await ItemService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
