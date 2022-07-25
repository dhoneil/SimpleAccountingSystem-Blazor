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
        public bool IsValidSubmit { get; set; } = true;
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
            IsValidSubmit = true;
            CurrentBranch = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (String.IsNullOrEmpty(CurrentBranch.BranchName))
            {
                IsValidSubmit = false;
            }
            else if (Branches.Where(s => s.BranchName == CurrentBranch.BranchName).ToList().Count() > 0)
            {
                IsValidSubmit = false;
            }
            else
            {
                IsValidSubmit=true;
            }

            if (IsValidSubmit)
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

            StateHasChanged();
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
