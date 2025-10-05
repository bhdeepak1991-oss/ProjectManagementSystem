using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using PMS.Domains;
using PMS.Extensions;
using PMS.Features.Master.Validators;
using PMS.Helpers;
using PMS.Notification;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FeatureRouteTransformer>();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

builder.Services.AddDbContext<PmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RoleValidator>());

builder.Services.AddDistributedMemoryCache(); // Required
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub");

app.MapDynamicControllerRoute<FeatureRouteTransformer>(
    "Features/{featureName}/{action=Index}/{id?}");

app.MapGet("/", context =>
{
    context.Response.Redirect("/Features/UserManagement/Authenticate");
    return Task.CompletedTask;
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
