using CanardEcarlate.Application;
using CanardEcarlate.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CanardEcarlate.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // MONGO
            services.Configure<UserstoreDatabaseSettings>(Configuration.GetSection(nameof(UserstoreDatabaseSettings)));
            services.AddSingleton<IUserstoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<UserstoreDatabaseSettings>>().Value);
            services.Configure<GamestoreDatabaseSettings>(Configuration.GetSection(nameof(GamestoreDatabaseSettings)));
            services.AddSingleton<IGamestoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<GamestoreDatabaseSettings>>().Value);
            services.Configure<UserStatisticsstoreDatabaseSettings>(Configuration.GetSection(nameof(UserStatisticsstoreDatabaseSettings)));
            services.AddSingleton<IUserStatisticsstoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<UserStatisticsstoreDatabaseSettings>>().Value);
            services.Configure<GlobalStatisticsstoreDatabaseSettings>(Configuration.GetSection(nameof(GlobalStatisticsstoreDatabaseSettings)));
            services.AddSingleton<IGlobalStatisticsstoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<GlobalStatisticsstoreDatabaseSettings>>().Value);
            services.Configure<CardsConfigurationUserstoreDatabaseSettings>(Configuration.GetSection(nameof(CardsConfigurationUserstoreDatabaseSettings)));
            services.AddSingleton<ICardsConfigurationUserstoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<CardsConfigurationUserstoreDatabaseSettings>>().Value);

            // REPOSITORIES
            services.AddSingleton<UserRepository>();

            // APPLICATION
            services.AddSingleton<UserService>();
            services.AddSingleton<GameService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CanardEcarlate.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                    new string[] {}
                }
                });
            });

            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "https://canardecarlate.fr",
                ValidAudience = "https://canardecarlate.fr",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ")),
                ClockSkew = TimeSpan.Zero // remove delay of token when expire
            };

            services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = TokenValidationParameters;
            });

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("Joueur", policy => policy.RequireClaim("type", "Joueur"));
                cfg.AddPolicy("ClearanceLevel1", policy => policy.RequireClaim("ClearanceLevel", "1"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CanardEcarlate.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
