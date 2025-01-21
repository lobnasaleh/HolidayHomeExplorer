using Microsoft.AspNetCore.Authentication.Cookies;
using Villa_Web.Services;
using Villa_Web.Services.IServices;

namespace Villa_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(typeof(MappingConfig));


            builder.Services.AddHttpClient<IVillaService, VillaService>();//

            builder.Services.AddScoped<IVillaService,VillaService>();

            builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();//

            builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();

            builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

            builder.Services.AddHttpClient<IAuthService, AuthService>();//

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddDistributedMemoryCache();
           
            //session
            builder.Services.AddSession(options => {
                options.IOTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            
            });
            //sets up cookie-based authentication for the application.
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                    //law el user msh authenticated ywedeeh 3ala el page de msh el default
                    options.LoginPath = "/Auth/Login";
                    //law el user not authorized
                    options.AccessDeniedPath= "/Auth/AccessDenied";
                });





            //  builder.Services.AddScoped<IBaseServicecs, BaseService>();

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

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
