using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SystemeLocation.Context;
using SystemeLocation.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//connection string 
builder.Services.AddDbContext<SystemeLocationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<Utilisateur, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<SystemeLocationContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
//adding fluent validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Configure the default path for the access to an action that has been denied.
builder.Services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/Index");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
