
using MagicVillaApI2.Data;
using MagicVillaApI2.Repositories;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using MagicVillaApI2.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

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

           /* builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

            });*/

           /* builder.Services.AddVersionedApiExplorer(options =>
            { options.GroupNameFormat = "'v'VVV"; });
*/
            builder.Services.AddEndpointsApiExplorer(); // This is needed for minimal APIs
            builder.Services.AddAutoMapper(typeof(MappingConfig));
           
            builder.Services.AddResponseCaching();

            builder.Services.AddControllers(Options => {
                Options.CacheProfiles.Add("Default30", new CacheProfile() { Duration = 30 });
            
            });

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IVillaRepository, VillaRepository>();
            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            /*
                        builder.Services.AddApiVersioning(options =>
                        {
                            options.AssumeDefaultVersionWhenUnspecified = true;
                            options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                            options.ReportApiVersions = true;
                        })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV"; // Formats the group names by version, e.g., v1, v2
                    options.SubstituteApiVersionInUrl = true; // Replaces the {version} in the route templates
                });*/
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
                x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("ApiSettings:Secret")))

                    };
                }

                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
