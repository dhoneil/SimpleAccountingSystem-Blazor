using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class CustomerPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] ICustomerService CustomerService { get; set; } = null!;
        public List<Customer> Customers { get; set; } = new();
        public Customer CurrentCustomer { get; set; } = new();
        public bool IsValidSubmit { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Customers = await CustomerService.GetCustomers();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "Customermodal", action);
        }

        public async Task AddNewLocation()
        {
            IsValidSubmit = true;
            CurrentCustomer = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (String.IsNullOrEmpty(CurrentCustomer.CustomerName))
            {
                IsValidSubmit = false;
            }
            else if (Customers.Where(s => s.CustomerName == CurrentCustomer.CustomerName).ToList().Count() > 0)
            {
                IsValidSubmit = false;
            }
            else
            {
                IsValidSubmit = true;
            }


            if (IsValidSubmit)
            {
                if (CurrentCustomer.CustomerId > 0)
                {
                    await CustomerService.UpdateItem(CurrentCustomer);
                }
                else
                {
                    await CustomerService.CreateItem(CurrentCustomer);
                }

                CurrentCustomer = new();
                await LoadData();
                await ModalAction("hide");
            }
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            var item = (Customer)args.Item;
            var locationid = item.CustomerId;
            CurrentCustomer = await CustomerService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
