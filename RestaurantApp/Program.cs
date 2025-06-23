using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Areas.Admin.Repositories;
using RestaurantApp.Data;

namespace RestaurantApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddScoped<ICategoryRepository , CategoryRepository>();
            builder.Services.AddScoped<IItemRepository , ItemRepository>();

            builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
             .AddRoles<AppRole>().AddEntityFrameworkStores<RestaurantDbContext>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/Account/Login";
        options.AccessDeniedPath = "/Admin/Account/AccessDenied";

    });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    var request = context.Request;

                    if (request.Path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Response.Redirect("/Admin/Account/Login?ReturnUrl=" + Uri.EscapeDataString(request.Path + request.QueryString));
                    }
                    else
                    {
                        context.Response.Redirect("Identity/Account/Login?ReturnUrl=" + Uri.EscapeDataString(request.Path + request.QueryString));
                    }

                    return Task.CompletedTask;
                };
            });

            builder.Services.AddControllersWithViews();
        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            
                app.MapControllerRoute(
         name: "areas",
         pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
               

            app.Run();
        }
    }
}
