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
        [Inject] IHelperService HelperService { get; set; } = null!;
        [Inject] IItemTransactionService ItemTransactionService { get; set; } = null!;
        [Inject] IBrandService BrandService { get; set; } = null!;
        [Inject] ICategoryService CategoryService { get; set; } = null!;
        [Inject] IPartNumberService PartNumberService { get; set; } = null!;
        [Inject] IUomService UomService { get; set; } = null!;
        [Inject] IItemService ItemService { get; set; } = null!;
        [Inject] IReceivedItemService ReceivedItemService { get; set; } = null!;
        [Inject] IReleasedItemService ReleasedItemService { get; set; } = null!;
        public List<Item> Items { get; set; } = new();
        public List<ItemTransaction> ItemTransactions { get; set; } = new();
        public List<PartNumber> PartNumbers { get; set; } = new();
        public List<Uom> Uoms { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
        public Item CurrentItem { get; set; } = new();
        public bool IsValidSubmit { get; set; } = true;
        public List<ReceivedItemDetail> ReceivedItemDetails { get; set; } = new();
        public List<ReleasedItemDetail> ReleasedItemDetails { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Brands = await BrandService.GetBrands();
            Categories = await CategoryService.GetCategories();
            Uoms = await UomService.GetUoms();
            PartNumbers = await PartNumberService.GetPartNumbers();
            Items = await ItemService.GetItems();
            ItemTransactions = await ItemTransactionService.GetItemTransactions();
        }

        public async Task ShowItemLedger(GridCommandEventArgs args)
        {
            var item = (Item)args.Item;
            var item_id = item.ItemId;
            var received_items_all = await ReceivedItemService.GetReceivedItemDetailsAsync(0);
            var released_items_all = await ReleasedItemService.GetReleasedItemDetailsAsync(0);
            var received_items = received_items_all.Where(s => s.ItemId == item_id).ToList();
            var released_items = released_items_all.Where(s => s.ItemId == item_id).ToList();
            ReceivedItemDetails = received_items;
            ReleasedItemDetails = released_items;
            await ModalAction("ItemLedgerModal", "show");
        }

        public decimal GetItemFinalCount(int itemid)
        {
            var item_transactions_count_in = ItemTransactions.Where(s => s.ItemId == itemid && s.TransactionTypeId == 1).Sum(s => s.Qty);
            var item_transactions_count_out = ItemTransactions.Where(s => s.ItemId == itemid && s.TransactionTypeId == 2).Sum(s => s.Qty);
            var final_count = item_transactions_count_in - item_transactions_count_out;
            return (decimal)final_count;
        }

        async Task ModalAction(string modalid,string action)
        {
            //await JS.InvokeVoidAsync("ModalAction", "Itemmodal", action);
            await HelperService.ModalAction($"{modalid}", action);
        }

        public async Task AddNewLocation()
        {
            IsValidSubmit = true;
            CurrentItem = new();
            await ModalAction("ItemModal","show");
        }

        public async Task SaveItem()
        {
            if (CurrentItem.ItemId <= 0)
            {
                if (String.IsNullOrEmpty(CurrentItem.ItemCode))
                {
                    IsValidSubmit = false;
                }
                else if (String.IsNullOrEmpty(CurrentItem.ItemDescription))
                {
                    IsValidSubmit = false;
                }
                else if (String.IsNullOrEmpty(Convert.ToString(CurrentItem.ReorderPoint)))
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
                    await ItemService.CreateItem(CurrentItem);
                }
            }
            else
            {
                await ItemService.UpdateItem(CurrentItem);
            }

            CurrentItem = new();
            await LoadData();
            await ModalAction("ItemModal", "hide");
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            var item = (Item)args.Item;
            var locationid = item.ItemId;
            CurrentItem = await ItemService.GetDetails(locationid);
            await ModalAction("ItemModal","show");
        }
    }
}
