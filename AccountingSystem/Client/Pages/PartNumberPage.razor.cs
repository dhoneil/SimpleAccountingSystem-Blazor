using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class PartNumberPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] IPartNumberService PartNumberService { get; set; } = null!;
        public List<PartNumber> PartNumbers { get; set; } = new();
        public PartNumber CurrentPartNumber { get; set; } = new();
        public bool IsValidSubmit { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            PartNumbers = await PartNumberService.GetPartNumbers();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "PartNumbermodal", action);
        }

        public async Task AddNewLocation()
        {
            IsValidSubmit = true;
            CurrentPartNumber = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (String.IsNullOrEmpty(CurrentPartNumber.PartNumberName))
            {
                IsValidSubmit = false;
            }
            else if (PartNumbers.Where(s => s.PartNumberName == CurrentPartNumber.PartNumberName && s.PartNumberSuffix == CurrentPartNumber.PartNumberSuffix).ToList().Count() > 0)
            {
                IsValidSubmit = false;
            }
            else
            {
                IsValidSubmit = true;
            }

            if (IsValidSubmit)
            {
                if (CurrentPartNumber.PartNumberId > 0)
                {
                    await PartNumberService.UpdateItem(CurrentPartNumber);
                }
                else
                {
                    await PartNumberService.CreateItem(CurrentPartNumber);
                }

                CurrentPartNumber = new();
                await LoadData();
                await ModalAction("hide");
            }
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            var item = (PartNumber)args.Item;
            var locationid = item.PartNumberId;
            CurrentPartNumber = await PartNumberService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
