using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Text;
using Microsoft.JSInterop;
using AccountingSystem.Shared.Utility;

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
        public List<ReceivedItem> ReceivedItems { get; set; } = new();
        public ReceivedItem CurrentReceivedItem { get; set; } = new();
        public List<Item> Items { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public Supplier CurrentSelectedSupplier { get; set; } = new();
        public List<ReceivedItemDetail> ReceivedItemDetailList { get; set; } = new();
        public ReceivedItemDetail CurrentReceivedItemDetail { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            CurrentReceivedItemDetail.Qty = 1;
            CurrentReceivedItem.ReceiveTransactionNo = HelperService.GenerateTransactionNo(8);
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

        public async Task AddNewReceivedItem()
        {
            await HelperService.ModalAction("ReceiveItemmodal", "show");
        }

        public async Task SaveItem()
        {

        }

        public async Task OnChangeSupplier(ChangeEventArgs selectedsupplier)
        {
            var selectedSuppId = selectedsupplier.Value.ToString();
            CurrentSelectedSupplier = Suppliers.Where(s => s.SupplierId == int.Parse(selectedSuppId))?.FirstOrDefault();
        }

        public async Task AddToReceivedItemDetailList()
        {
            var is_already_added = ReceivedItemDetailList.Where(s=>s.ItemId == CurrentReceivedItemDetail.ItemId).Any();
            if (!is_already_added)
            {
                ReceivedItemDetailList.Add(CurrentReceivedItemDetail);
                CurrentReceivedItemDetail = new();
                CurrentReceivedItemDetail.Qty = 1;
                StateHasChanged();
            }
            else
            {
                await JS.InvokeVoidAsync("alert","Item already added");
                return;
            }
        }

        public async Task RemoveFromReceivedItemDetailList(int detailid)
        {
            var toremove = ReceivedItemDetailList.FirstOrDefault(s => s.ReceivedItemDetailId == detailid);
            ReceivedItemDetailList.Remove(toremove);
        }

        public async Task SaveReceiveItem()
        {
            var base_url = _http.BaseAddress.AbsoluteUri;
            CurrentReceivedItem.SupplierId = CurrentSelectedSupplier.SupplierId;
            var result = await ReceivedItemService.CreateItem(CurrentReceivedItem);
            var latest_inserted = await ReceivedItemService.GetLastReceiveItem();
            foreach (var x in ReceivedItemDetailList)
            {
                x.ReceivedItemId = latest_inserted.ReceivedItemId;
                await ReceivedItemService.AddNewReceiveItemDetail(x);
            }
        }
    }
}
