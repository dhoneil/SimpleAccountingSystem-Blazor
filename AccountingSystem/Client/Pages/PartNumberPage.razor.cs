﻿using AccountingSystem.Client.Services;
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
            CurrentPartNumber = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
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

        public async Task EditItem(GridCommandEventArgs args)
        {
            var item = (PartNumber)args.Item;
            var locationid = item.PartNumberId;
            CurrentPartNumber = await PartNumberService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
