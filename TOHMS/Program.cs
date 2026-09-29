using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TOHMS.Data;
using TOHMS.Models;
using TOHMS.Repositories;
using TOHMS.Repositories.Interfaces;
using TOHMS.Services;
using TOHMS.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// MVC + API
// =====================================================

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// =====================================================
// DATABASE
// =====================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")
    ));

// =====================================================
// IDENTITY / AUTHENTICATION
// =====================================================

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;

        options.Password.RequireDigit = true;

        options.Password.RequireLowercase = true;

        options.Password.RequireUppercase = true;

        options.Password.RequireNonAlphanumeric = true;

        options.User.RequireUniqueEmail = true;

        options.Lockout.MaxFailedAccessAttempts = 5;

        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(5);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";

    options.AccessDeniedPath =
        "/Account/AccessDenied";

    options.ExpireTimeSpan =
        TimeSpan.FromHours(8);

    options.SlidingExpiration = true;
});

// =====================================================
// REPOSITORIES
// =====================================================

builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));

builder.Services.AddScoped<
    IClientRepository,
    ClientRepository>();

builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

// =====================================================
// SERVICES
// =====================================================

builder.Services.AddScoped<
    IClientService,
    ClientService>();

builder.Services.AddScoped<
    IDashboardService,
    DashboardService>();

// =====================================================
// BUILD APPLICATION
// =====================================================

var app = builder.Build();

// =====================================================
// HTTP PIPELINE
// =====================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

// =====================================================
// SWAGGER
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseRouting();

// IMPORTANT:
// Authentication must come before Authorization.

app.UseAuthentication();

app.UseAuthorization();

// =====================================================
// ROUTING
// =====================================================

app.MapControllerRoute(
    name: "default",
    pattern:
        "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

// =====================================================
// SEED ROLES + DEVELOPMENT ADMIN
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var services =
        scope.ServiceProvider;

    await DbInitializer.SeedRolesAndAdminAsync(
        services);
}

// =====================================================
// RUN
// =====================================================

app.Run();