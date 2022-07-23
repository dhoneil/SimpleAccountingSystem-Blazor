using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using AccountingSystem.Client.Services;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class ContractPage : ComponentBase
    {
        [Inject] IContractService ContractService { get; set; } = null!;
        public List<Contract> ContractsList { get; set; } = new();
        public Random Random { get; set; } = new();
        public string CurrentContractNo { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            ContractsList = await ContractService.GetContractsAsync();
        }

        private async Task CreateHandler(GridCommandEventArgs args)
        {
            var entity = (Contract)args.Item;
            entity.ContractNo = CurrentContractNo;
            var newentity = await ContractService.CreateContract(entity);

            CurrentContractNo = string.Empty;
            await LoadData();
        }

        private async Task UpdateHandler(GridCommandEventArgs args)
        {
            var entity = (Contract)args.Item;
            ContractsList[ContractsList.FindIndex(c => c.Id == entity.Id)] = entity;
            await ContractService.UpdateContract(entity);
        }

        public async void GenerateContractNo()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var res = new string(Enumerable.Repeat(chars, 13)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
            CurrentContractNo = res;
        }
    }
}
