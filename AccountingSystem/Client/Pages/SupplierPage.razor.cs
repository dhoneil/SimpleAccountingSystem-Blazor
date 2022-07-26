using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class SupplierPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] ISupplierService SupplierService { get; set; } = null!;
        public List<Supplier> Suppliers { get; set; } = new();
        public Supplier CurrentSupplier { get; set; } = new();
        public bool IsValidSubmit { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Suppliers = await SupplierService.GetSuppliers();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "Suppliermodal", action);
        }

        public async Task AddNewLocation()
        {
            IsValidSubmit = true;
            CurrentSupplier = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (String.IsNullOrEmpty(CurrentSupplier.SupplierName))
            {
                IsValidSubmit = false;
            }
            if (String.IsNullOrEmpty(CurrentSupplier.ContactPerson))
            {
                IsValidSubmit = false;
            }
            if (String.IsNullOrEmpty(CurrentSupplier.ContactNo))
            {
                IsValidSubmit = false;
            }
            if (String.IsNullOrEmpty(CurrentSupplier.Address))
            {
                IsValidSubmit = false;
            }
            else if (Suppliers.Where(s => s.SupplierName == CurrentSupplier.SupplierName && s.Address == CurrentSupplier.Address).ToList().Count() > 0)
            {
                IsValidSubmit = false;
            }
            else
            {
                IsValidSubmit = true;
            }


            if (IsValidSubmit)
            {
                if (CurrentSupplier.SupplierId > 0)
                {
                    await SupplierService.UpdateItem(CurrentSupplier);
                }
                else
                {
                    await SupplierService.CreateItem(CurrentSupplier);
                }

                CurrentSupplier = new();
                await LoadData();
                await ModalAction("hide");
            }
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            IsValidSubmit = true;
            var item = (Supplier)args.Item;
            var locationid = item.SupplierId;
            CurrentSupplier = await SupplierService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
