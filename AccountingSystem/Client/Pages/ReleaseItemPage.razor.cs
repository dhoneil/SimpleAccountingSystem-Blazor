using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Text;
using Microsoft.JSInterop;
using AccountingSystem.Shared.Utility;
using Telerik.Blazor;
using AccountingSystem.Shared.GlobalVariables;
using AccountingSystem.Shared.GlobalConstants;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class ReleaseItemPage : ComponentBase
    {
        [Inject] HttpClient _http { get; set; } = null!;
        [Inject] IHelperService HelperService { get; set; } = null!;
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] IReleasedItemService ReleasedItemService { get; set; } = null!;
        [Inject] IItemService ItemService { get; set; } = null!;
        [Inject] ICustomerService CustomerService { get; set; } = null!;
        [Inject] ILocationService LocationService { get; set; } = null!;
        [Inject] IItemTransactionService ItemTransactionService { get; set; } = null!;
        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        public List<ReleasedItem> ReleasedItems { get; set; } = new();
        public ReleasedItem CurrentReleasedItem { get; set; } = new();
        public ReleasedItem CurrentReleasedItemToViewDetails { get; set; } = new();
        public List<Item> Items { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public Customer CurrentSelectedCustomer { get; set; } = new();
        public List<ReleasedItemDetail> ReleasedItemDetailList { get; set; } = new();
        public List<ReleasedItemDetail> TransactionDetails { get; set; } = new();
        public ReleasedItemDetail CurrentReleasedItemDetail { get; set; } = new();
        public bool IsTransactionDetailModalVisible { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            CurrentReleasedItemDetail.Qty = 1;
            GenerateTransactionNo();
        }

        void GenerateTransactionNo()
        {
            CurrentReleasedItem.ReleaseTransactionNo = HelperService.GenerateTransactionNo(8);
            CurrentReleasedItem.DateReleased = DateTime.Now;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JS.InvokeVoidAsync("Select2Init");
        }

        public async Task LoadData()
        {
            ReleasedItems = await ReleasedItemService.GetReleasedItems();
            Items = await ItemService.GetItems();
            Customers = await CustomerService.GetCustomers();
            Locations = await LocationService.GetLocations();
        }

        public async Task OnChangeLocation(object selectedlocation)
        {
            Items = await ItemService.GetItems();
            var location_id = (int)selectedlocation;
            var location_details = await LocationService.GetDetails(location_id);
            var location_category_id = location_details.CategoryId;
            var items_with_same_category = Items.Where(s=>s.CategoryId == location_category_id).ToList();
            Items = (items_with_same_category is null ? Items = new() : items_with_same_category);
            StateHasChanged();
        }

        public async Task OnChangeItem(object selecteditem)
        {
            var item_id = (int)selecteditem;
            var item_details = await ItemService.GetDetails(item_id);
            CurrentReleasedItemDetail.Cost = item_details.UnitCost;
        }

        public async Task AddNewReleasedItem()
        {
            await HelperService.ModalAction("ReleaseItemmodal", "show");
        }

        public async Task OnChangeCustomer(object selectedCustomer)
        {
            Customers = await CustomerService.GetCustomers();
            var selectedSuppId = (int)selectedCustomer;
            CurrentSelectedCustomer = Customers.Where(s => s.CustomerId == selectedSuppId)?.FirstOrDefault();
        }

        public async Task AddToReleasedItemDetailList()
        {
            bool is_valid = true;
            var is_already_added = ReleasedItemDetailList.Where(s=>s.ItemId == CurrentReleasedItemDetail.ItemId).Any();
            if (is_already_added)
            {
                await JS.InvokeVoidAsync("alert", "Item already added");
                return;
            }
            if (CurrentReleasedItemDetail.ItemId is null)
            {
                await JS.InvokeVoidAsync("alert", "Please select an Item");
                return;
            }
            if (is_valid)
            {
                ReleasedItemDetailList.Add(CurrentReleasedItemDetail);
                CurrentReleasedItemDetail = new();
                CurrentReleasedItemDetail.Qty = 1;
                StateHasChanged();
            }
        }

        public async Task RemoveFromReleasedItemDetailList(int detailid)
        {
            var toremove = ReleasedItemDetailList.FirstOrDefault(s => s.ReleasedItemDetailId == detailid);
            ReleasedItemDetailList.Remove(toremove);
        }

        public async Task SaveReleaseItem()
        {
            await HelperService.ModalAction("ReViewReleaseItemModel","show");
            //bool isConfirmed = await Dialogs.ConfirmAsync("Are you sure to save this?",GlobalVariables.CONFIRMWINDOWTITLE);

            // await FinalSaveReleaseItem();
        }

        public async Task FinalSaveReleaseItem()
        {
            //ADD TO RECIEVE ITEM TABLE
            CurrentReleasedItem.CustomerId = CurrentSelectedCustomer.CustomerId;
            await ReleasedItemService.CreateItem(CurrentReleasedItem);
            var latest_inserted = await ReleasedItemService.GetLastReleaseItem();

            //ADD TO RELEASE ITEM DETAIL TABLE
            List<ReleasedItemDetail> ReleasedItemDetails = new List<ReleasedItemDetail>();
            foreach (var x in ReleasedItemDetailList)
            {
                x.ReleasedItemId = latest_inserted.ReleasedItemId;
                x.LocationId = (x.LocationId is null ? 0 : x.LocationId);
                ReleasedItemDetails.Add(x);
            }
            await ReleasedItemService.AddNewReleaseItemDetails(ReleasedItemDetails);

            //ADD TO ITEM TRANSACTION TABLE
            List<ItemTransaction> ItemTransactions = new List<ItemTransaction>();
            foreach (var y in ReleasedItemDetails)
            {
                ItemTransaction transaction = new ItemTransaction();
                transaction.ItemId = y.ItemId;
                transaction.TransactionTypeId = 2; // OUT
                transaction.Qty = y.Qty;
                ItemTransactions.Add(transaction);
            }
            await ItemTransactionService.CreateManyItem(ItemTransactions);

            CurrentReleasedItem = new();
            ReleasedItemDetailList = new();
            CurrentSelectedCustomer = new();
            GenerateTransactionNo();
        }

        public async Task ShowTransactionDetails(GridCommandEventArgs args)
        {
            var Releaseitem = (ReleasedItem)args.Item;
            var result = await ReleasedItemService.GetReleasedItemDetailsAsync(Releaseitem.ReleasedItemId);
            CurrentReleasedItemToViewDetails = Releaseitem;
            TransactionDetails = result;
            await HelperService.ModalAction("Releasedetailtransactionmodel","show");
        }
    }
}
