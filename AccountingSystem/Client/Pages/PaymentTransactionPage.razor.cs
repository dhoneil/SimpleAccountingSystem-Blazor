using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using AccountingSystem.Client.Services;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class PaymentTransactionPage : ComponentBase
    {
        [Inject] IPaymentTransactionService PaymentTransactionService { get; set; } = null!;
        [Inject] IContractService ContractService { get; set; } = null!;
        public List<Contract> ContractsList { get; set; } = new();
        public List<PaymentTransaction> PaymentTransactionsList { get; set; } = new();
        public bool IsPaymentFormModelVisible { get; set; } = false;
        public Contract SelectedContract { get; set; } = new();
        public PaymentTransaction SelectedPaymentTransaction { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            ContractsList = await ContractService.GetContractsAsync();
            PaymentTransactionsList = await PaymentTransactionService.GetPaymentTransactionsAsync();
        }

        private async Task CreateHandler(GridCommandEventArgs args)
        {
            var entity = (PaymentTransaction)args.Item;
            var newentity = await PaymentTransactionService.CreatePaymentTransaction(entity);
            await LoadData();
        }

        private async Task UpdateHandler(GridCommandEventArgs args)
        {
            var entity = (PaymentTransaction)args.Item;
            PaymentTransactionsList[PaymentTransactionsList.FindIndex(c => c.Id == entity.Id)] = entity;
            await PaymentTransactionService.UpdatePaymentTransaction(entity);
        }

        public async Task AddNewPayment()
        {
            IsPaymentFormModelVisible = true;
        }

        public async Task OnChangeContractNo(object contractid)
        {
            var selectedcontract = Convert.ToInt32(contractid);
            var res = await ContractService.GetDetails(selectedcontract);
            SelectedContract = res;
        }
    }
}
