using LibraryWeb;
using LibraryWeb.Services;
using LibraryWeb.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IBaseService,BaseService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IBookService,BookService>();
builder.Services.AddScoped<ILocationService,LocationService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/Auth/Login";
                  options.AccessDeniedPath = "/Auth/AccessDenied";
                  options.SlidingExpiration = true;
              });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
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

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "BooksByCategory",
    pattern: "{controller=Book}/{action=BookByCategory}/{categoryId?}");
app.MapControllerRoute(
    name: "BookPage",
    pattern: "{controller=Book}/{action=BookPage}/{bookId?}");

app.Run();
