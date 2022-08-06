using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.ViewModels.MySql;
using Microsoft.AspNetCore.Components;
using ClassLibrary;
using Microsoft.Extensions.Configuration;


namespace AccountingSystem.Client.Pages
{
    public partial class ExpensePage : ComponentBase
    {
        [Inject] IConfiguration Configuration { get; set; }
        [Inject] IDataService DataService { get;set;} = null!;
        [Inject] IDataAccess DataAccess { get;set;} = null!;
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
            //Customers = await DataAccess.LoadData<Customer, dynamic>("select customer_id, user_id from customers", new { }, "Server=localhost; Port=3306; Database=bookingsystem; Uid=root; Pwd=");
            //Customers = await DataService.LoadData<Customer,dynamic>("select customer_id, user_id from customers", new { }, "Server=localhost; Port=3306; Database=bookingsystem; Uid=root; Pwd=");
            //Customers = await DataService.GetAll<Customer>("select customer_id, user_id from customers", "Server=localhost; Port=3306; Database=bookingsystem; Uid=root; Pwd=");
            var connstring = "Server=en1ehf30yom7txe7.cbetxkdyhwsb.us-east-1.rds.amazonaws.com;Port=3306;database=yy7wz6uh9veb7enp;user id=tog84xlx7kgvdqrz;password=bts3yjjs3fmjgpvt";
            var custs = await DataAccess.LoadData<User, dynamic>("select name, usertype from users", new { }, connstring);
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
