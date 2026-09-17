using Microsoft.EntityFrameworkCore;
using Shop_Admin.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    ));

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapRazorPages();

app.Run();