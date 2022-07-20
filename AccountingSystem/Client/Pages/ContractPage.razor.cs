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
            var newentity = await ContractService.CreateContract(entity);
            await LoadData();
        }

        private async Task UpdateHandler(GridCommandEventArgs args)
        {
            var entity = (Contract)args.Item;
            ContractsList[ContractsList.FindIndex(c => c.Id == entity.Id)] = entity;
            await ContractService.UpdateContract(entity);
        }
    }
}
