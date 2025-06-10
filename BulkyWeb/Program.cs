using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bulky.Utility;
using Bulky.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// for Stripe 
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

// for DB context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// for identity Role and user management
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// for access denied page
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// for facebook authentication
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "1394601501572999";
    option.AppSecret = "b5c1f0ea782637d98df47112b940aaf5";
});

// for microsoft authentication
builder.Services.AddAuthentication().AddMicrosoftAccount(option =>
{
    option.ClientId = "29abcb30-17bb-447e-8dc0-965c038f4bc7";
    option.ClientSecret = "YK18Q~JPq5MaBnFtEpygW2gfQvaeoxmjMOYx.aRP";
});


// for session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100); // Set session timeout to 100 minutes
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential for the application
});

// for Initializer
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
// for razor pages
builder.Services.AddRazorPages();
// for repository pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// for email sender service
builder.Services.AddScoped<IEmailSender, EmailSender>();


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
Stripe.StripeConfiguration.ApiKey= builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
// for session management
app.UseSession();
// for Initializeer
SeedDatabase();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

// Initialize the database
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
