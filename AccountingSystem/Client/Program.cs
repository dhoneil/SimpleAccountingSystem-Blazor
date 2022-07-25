using AccountingSystem.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AccountingSystem.Client.Services;
using ClassLibrary;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
builder.Services.AddScoped(sp => http);

builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IPaymentTransactionService, PaymentTransactionService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IPartNumberService, PartNumberService>();
builder.Services.AddScoped<IUomService, UomService>();

builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddSingleton<IDataAccess, DataAccess>();





builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
