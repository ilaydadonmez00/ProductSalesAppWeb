using Microsoft.EntityFrameworkCore;
using Mikro.ProductSalesWeb.Domain.Entity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Sales}/{action=Home}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Products.Any())
    {
        var initialProducts = new List<Product>
        {
            new Product { Barcode = "4001", Name = "Milk (1L)", Quantity = 100, UnitPrice = 25.00m },
            new Product { Barcode = "4002", Name = "Eggs (10 pcs)", Quantity = 80, UnitPrice = 45.00m },
            new Product { Barcode = "4003", Name = "Sugar (1kg)", Quantity = 60, UnitPrice = 50.00m },
            new Product { Barcode = "4004", Name = "Sunflower Oil (1L)", Quantity = 40, UnitPrice = 95.00m },
            new Product { Barcode = "4005", Name = "Pasta (500g)", Quantity = 150, UnitPrice = 20.00m },
            new Product { Barcode = "4006", Name = "Rice (1kg)", Quantity = 90, UnitPrice = 60.00m },
            new Product { Barcode = "4007", Name = "Tea (500g)", Quantity = 70, UnitPrice = 85.00m },
            new Product { Barcode = "4008", Name = "Coffee (100g)", Quantity = 50, UnitPrice = 120.00m },
            new Product { Barcode = "4009", Name = "Biscuits (pack)", Quantity = 200, UnitPrice = 15.00m },
            new Product { Barcode = "4010", Name = "Cheese (250g)", Quantity = 60, UnitPrice = 80.00m },
            new Product { Barcode = "4011", Name = "Apple (kg)", Quantity = 100, UnitPrice = 35.00m },
            new Product { Barcode = "4012", Name = "Banana (kg)", Quantity = 80, UnitPrice = 40.00m },
            new Product { Barcode = "4013", Name = "Tomato (kg)", Quantity = 120, UnitPrice = 25.00m }
        };

        context.Products.AddRange(initialProducts);
        context.SaveChanges();
    }
}

app.Run();

