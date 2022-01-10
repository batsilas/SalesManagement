using Microsoft.EntityFrameworkCore;
using SalesManagement.Data;
using SalesManagement.Services;
using SellersManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SalesManagementDbContext>(options =>
    options.UseSqlServer(connectionString)); 
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISellerService, SellerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
