using AccountingSystem.Client.Services;
using AccountingSystem.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace AccountingSystem.Client.Pages
{
    public partial class CategoryPage : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] ICategoryService CategoryService { get; set; } = null!;
        public List<Category> Categorys { get; set; } = new();
        public Category CurrentCategory { get; set; } = new();
        public bool IsValidSubmit { get; set; } = true;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        async Task LoadData()
        {
            Categorys = await CategoryService.GetCategories();
        }
        
        async Task ModalAction(string action)
        {
            await JS.InvokeVoidAsync("ModalAction", "Categorymodal", action);
        }

        public async Task AddNewLocation()
        {
            IsValidSubmit = true;
            CurrentCategory = new();
            await ModalAction("show");
        }

        public async Task SaveItem()
        {
            if (String.IsNullOrEmpty(CurrentCategory.CategoryName))
            {
                IsValidSubmit = false;
            }
            else if (Categorys.Where(s => s.CategoryName == CurrentCategory.CategoryName).ToList().Count() > 0)
            {
                IsValidSubmit = false;
            }
            else
            {
                IsValidSubmit = true;
            }


            if (IsValidSubmit)
            {
                if (CurrentCategory.CategoryId > 0)
                {
                    await CategoryService.UpdateItem(CurrentCategory);
                }
                else
                {
                    await CategoryService.CreateItem(CurrentCategory);
                }

                CurrentCategory = new();
                await LoadData();
                await ModalAction("hide");
            }
        }

        public async Task EditItem(GridCommandEventArgs args)
        {
            IsValidSubmit = true;
            var item = (Category)args.Item;
            var locationid = item.CategoryId;
            CurrentCategory = await CategoryService.GetDetails(locationid);
            await ModalAction("show");
        }
    }
}
