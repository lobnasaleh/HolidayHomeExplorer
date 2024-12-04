
using MagicVillaApI2.Data;
using MagicVillaApI2.Repositories;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using MagicVillaApI2.Models;

namespace MagicVillaApI2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            /*  Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()//ba2olo ana 3yza eh noo3 el haga ely ttketb 
                  .WriteTo.File("log/mylog.txt",rollingInterval:RollingInterval.Day).CreateLogger();//when a new file should be created //infinte will use same file forever


              builder.Host.UseSerilog();  //3yza astakhdem serilog badal el default console logging
  */
            builder.Services.AddAutoMapper(typeof(MappingConfig));

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            builder.Services.AddScoped<IVillaRepository, VillaRepository>();
            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
