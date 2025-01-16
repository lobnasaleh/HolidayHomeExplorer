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


            //  builder.Services.AddScoped<IBaseServicecs, BaseService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
