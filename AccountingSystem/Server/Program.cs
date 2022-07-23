using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using AccountingSystem.Server.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//setup database connection
var CONNECTIONSTRING = builder.Configuration.GetConnectionString("aquipcebuconnection");
builder.Services.AddDbContext<accounting_systemContext>(options => options.UseSqlServer(CONNECTIONSTRING),ServiceLifetime.Transient);

builder.Services.AddTelerikBlazor();




builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
