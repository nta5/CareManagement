using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CareManagement.Data;
using CareManagement.Models;
using EmailService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CareManagementContext>(options =>
    options.
    /*UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));*/
    UseInMemoryDatabase(databaseName: "CareManagement"));

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}


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
