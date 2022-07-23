using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class ExpensePage : ComponentBase
    {
        [Inject] IContractService ContractService { get;set;} = null!;
        [Inject] IExpenseService ExpenseService { get;set;} = null!;
        public List<Contract> ContractList { get; set; } = new List<Contract>();
        public List<Expense> ExpenseList { get; set; } = new List<Expense>();
        public Expense Expense { get; set; } = new Expense();
        public bool IsExpenseModalVisible { get; set; } = false;
        public Contract SelectedContract { get; set; } = new();
        public Random Random { get; set; } = new();
        public string CurrentTransactionNo { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            ContractList = await ContractService.GetContractsAsync();
            ExpenseList = await ExpenseService.GetExpensesAsync();
        }

        public void GenerateTransactionNo()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var res = new string(Enumerable.Repeat(chars, 13)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
            CurrentTransactionNo = res;
        }

        public async Task AddNewExpense()
        {
            GenerateTransactionNo();
            IsExpenseModalVisible = true;
        }

        async Task OnChangeContract(object contract_id)
        {
            var contractid = (int)contract_id;
            SelectedContract = await ContractService.GetDetails(contractid);
        }


    }
}
