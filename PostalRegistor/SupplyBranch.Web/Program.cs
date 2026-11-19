using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using SupplyBranch.Helpers;
using SupplyBranch.DataAccess;
using SupplyBranch.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Services
builder.Services.AddControllersWithViews();

// Require authenticated users by default; allow anonymous on specific endpoints (e.g., Account/Login)
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// App services
builder.Services.AddSingleton<DBHelper>();
builder.Services.AddScoped<SupplyDAL>();
builder.Services.AddScoped<IndentDAL>();
builder.Services.AddScoped<OfficeDAL>();
builder.Services.AddScoped<StockDAL>();
builder.Services.AddScoped<UserDAL>();
builder.Services.AddScoped<SupplyNumberGenerator>();

// Authentication and current user
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        // Make cookie usable in typical browser flows (allow SameSite lax for POST redirects, keep HttpOnly)
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// In Development, ensure a test user exists so login can be tested easily.
if (app.Environment.IsDevelopment())
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var userDal = scope.ServiceProvider.GetRequiredService<SupplyBranch.DataAccess.UserDAL>();

            // Test credentials (development only)
            const string testUserName = "test";
            const string testPassword = "test123";
            const string testFullName = "Test User";
            const string testEmail = "test@example.com";

            if (!userDal.UserNameExists(testUserName))
            {
                userDal.AddUser(testFullName, testUserName, testPassword, testEmail, "1");
            }
        }
    }
    catch
    {
        // ignore startup test user creation failures
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
