using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class BranchPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] IBranchService BranchService { get; set; } = null!;
        public List<Branch> Branches { get; set; } = new();
        public Branch CurrentBranch { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Branches = await BranchService.GetBranchs();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "branchmodal", action);
        }

        public async Task AddNewLocation()
        {
            CurrentBranch = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (CurrentBranch.BranchId > 0)
            {
                await BranchService.UpdateItem(CurrentBranch);
            }
            else
            {
                await BranchService.CreateItem(CurrentBranch);
            }

            CurrentBranch = new();
            await LoadData();
            await ModalAction("hide");
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            var item = (Branch)args.Item;
            var locationid = item.BranchId;
            CurrentBranch = await BranchService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
