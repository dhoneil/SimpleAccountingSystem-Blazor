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
    public partial class ReceiveItemPage : ComponentBase
    {
        [Inject] HttpClient _http { get; set; } = null!;
        [Inject] IHelperService HelperService { get; set; } = null!;
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] IReceivedItemService ReceivedItemService { get; set; } = null!;
        [Inject] IItemService ItemService { get; set; } = null!;
        [Inject] ISupplierService SupplierService { get; set; } = null!;
        [Inject] ILocationService LocationService { get; set; } = null!;
        [Inject] IItemTransactionService ItemTransactionService { get; set; } = null!;
        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        public List<ReceivedItem> ReceivedItems { get; set; } = new();
        public ReceivedItem CurrentReceivedItem { get; set; } = new();
        public ReceivedItem CurrentReceivedItemToViewDetails { get; set; } = new();
        public List<Item> Items { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public Supplier CurrentSelectedSupplier { get; set; } = new();
        public List<ReceivedItemDetail> ReceivedItemDetailList { get; set; } = new();
        public List<ReceivedItemDetail> TransactionDetails { get; set; } = new();
        public ReceivedItemDetail CurrentReceivedItemDetail { get; set; } = new();
        public bool IsTransactionDetailModalVisible { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            CurrentReceivedItemDetail.Qty = 1;
            GenerateTransactionNo();
        }

        void GenerateTransactionNo()
        {
            CurrentReceivedItem.ReceiveTransactionNo = HelperService.GenerateTransactionNo(8);
            CurrentReceivedItem.DateReceived = DateTime.Now;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JS.InvokeVoidAsync("Select2Init");
        }

        public async Task LoadData()
        {
            ReceivedItems = await ReceivedItemService.GetReceivedItems();
            Items = await ItemService.GetItems();
            Suppliers = await SupplierService.GetSuppliers();
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
            CurrentReceivedItemDetail.Cost = item_details.UnitCost;
        }

        public async Task AddNewReceivedItem()
        {
            await HelperService.ModalAction("ReceiveItemmodal", "show");
        }

        public async Task OnChangeSupplier(object selectedsupplier)
        {
            Suppliers = await SupplierService.GetSuppliers();
            var selectedSuppId = (int)selectedsupplier;
            CurrentSelectedSupplier = Suppliers.Where(s => s.SupplierId == selectedSuppId)?.FirstOrDefault();
        }

        public async Task AddToReceivedItemDetailList()
        {
            bool is_valid = true;
            var is_already_added = ReceivedItemDetailList.Where(s=>s.ItemId == CurrentReceivedItemDetail.ItemId).Any();
            if (is_already_added)
            {
                await JS.InvokeVoidAsync("alert", "Item already added");
                return;
            }
            if (CurrentReceivedItemDetail.ItemId is null)
            {
                await JS.InvokeVoidAsync("alert", "Please select an Item");
                return;
            }
            if (is_valid)
            {
                ReceivedItemDetailList.Add(CurrentReceivedItemDetail);
                CurrentReceivedItemDetail = new();
                CurrentReceivedItemDetail.Qty = 1;
                StateHasChanged();
            }
        }

        public async Task RemoveFromReceivedItemDetailList(int detailid)
        {
            var toremove = ReceivedItemDetailList.FirstOrDefault(s => s.ReceivedItemDetailId == detailid);
            ReceivedItemDetailList.Remove(toremove);
        }

        public async Task SaveReceiveItem()
        {
            bool isConfirmed = await Dialogs.ConfirmAsync("Are you sure to save this?",GlobalVariables.CONFIRMWINDOWTITLE);

            if (isConfirmed)
            {
                //ADD TO RECIEVE ITEM TABLE
                CurrentReceivedItem.SupplierId = CurrentSelectedSupplier.SupplierId;
                await ReceivedItemService.CreateItem(CurrentReceivedItem);
                var latest_inserted = await ReceivedItemService.GetLastReceiveItem();

                //ADD TO RECIEVE ITEM DETAIL TABLE
                List<ReceivedItemDetail> receivedItemDetails = new List<ReceivedItemDetail>();
                foreach (var x in ReceivedItemDetailList)
                {
                    x.ReceivedItemId = latest_inserted.ReceivedItemId;
                    receivedItemDetails.Add(x);
                }
                await ReceivedItemService.AddNewReceiveItemDetails(receivedItemDetails);

                //ADD TO ITEM TRANSACTION TABLE
                List<ItemTransaction> ItemTransactions = new List<ItemTransaction>();
                foreach (var y in receivedItemDetails)
                {
                    ItemTransaction transaction = new ItemTransaction();
                    transaction.ItemId = y.ItemId;
                    transaction.TransactionTypeId = 1; // IN
                    transaction.Qty = y.Qty;
                    ItemTransactions.Add(transaction);
                }
                await ItemTransactionService.CreateManyItem(ItemTransactions);

                CurrentReceivedItem = new();
                ReceivedItemDetailList = new();
                CurrentSelectedSupplier = new();
                GenerateTransactionNo();
            }
            else
            {
                return;
            }
        }

        public async Task ShowTransactionDetails(GridCommandEventArgs args)
        {
            var receiveitem = (ReceivedItem)args.Item;
            var result = await ReceivedItemService.GetReceivedItemDetailsAsync(receiveitem.ReceivedItemId);
            CurrentReceivedItemToViewDetails = receiveitem;
            TransactionDetails = result;
            await HelperService.ModalAction("receivedetailtransactionmodel","show");
        }
    }
}
