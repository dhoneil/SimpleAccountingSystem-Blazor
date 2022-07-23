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
        public PaymentTransaction CurrentPayment { get; set; } = new();
        public Random Random { get; set; } = new();
        public string CurrentTransationNo { get; set; } = string.Empty;

        public string CurrentContractBalance { get; set; } = string.Empty;
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

        public void GenerateTransactionNo()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var res = new string(Enumerable.Repeat(chars, 13)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
            CurrentTransationNo = res;
        }

        public async Task AddNewPayment()
        {
            SelectedContract = new();
            CurrentPayment = new();
            CurrentContractBalance = string.Empty;
            GenerateTransactionNo();
            IsPaymentFormModelVisible = true;
        }

        public async Task OnChangeContractNo(object contractid)
        {
            var selectedcontract = Convert.ToInt32(contractid);
            var res = await ContractService.GetDetails(selectedcontract);

            var totalpaidsofar = await ContractService.GetTotalPaidSoFar(selectedcontract);

            SelectedContract = res;
            CurrentContractBalance = Convert.ToString(SelectedContract.TotalAmount - decimal.Parse(totalpaidsofar));
        }

        public async Task SavePayment()
        {
            CurrentPayment.ContractId = SelectedContract.Id;
            CurrentPayment.TransactionNo = CurrentTransationNo;
            CurrentPayment.PaymentDate = DateTime.Now;
            var res = await PaymentTransactionService.CreatePaymentTransaction(CurrentPayment);

            CurrentTransationNo = string.Empty;
            IsPaymentFormModelVisible = false;
            SelectedContract = new();
            await LoadData();
        }
    }
}
