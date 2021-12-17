using CanardEcarlate.Application;
using CanardEcarlate.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CanardEcarlate.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using AutoMapper;
using CanardEcarlate.Api.Mappings;

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
            services.AddCors(options => options
                .AddPolicy("CorsPolicy", builder => 
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()));
            services.AddSingleton<UserRepository>();

            services.AddSingleton<RoomRepository>();

            services.AddSingleton<AuthenticationService>();
            services.AddSingleton<RoomService>();
            services.AddControllers();
            MongoService(services);
            SwaggerService(services);
            AutoMapperService(services);
            AuthenticationAuthorisationService(services);
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
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
        }

        private void MongoService(IServiceCollection services)
        {
            services.Configure<UserStoreDatabaseSettings>(Configuration.GetSection(nameof(UserStoreDatabaseSettings)));
            services.AddSingleton<IUserStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<UserStoreDatabaseSettings>>().Value);

            services.Configure<RoomStoreDatabaseSettings>(Configuration.GetSection(nameof(RoomStoreDatabaseSettings)));
            services.AddSingleton<IRoomStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<RoomStoreDatabaseSettings>>().Value);

            services.Configure<UserStatisticsStoreDatabaseSettings>(Configuration.GetSection(nameof(UserStatisticsStoreDatabaseSettings)));
            services.AddSingleton<IUserStatisticsStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<UserStatisticsStoreDatabaseSettings>>().Value);
            
            services.Configure<GlobalStatisticsStoreDatabaseSettings>(Configuration.GetSection(nameof(GlobalStatisticsStoreDatabaseSettings)));
            services.AddSingleton<IGlobalStatisticsStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<GlobalStatisticsStoreDatabaseSettings>>().Value);
            
            services.Configure<CardsConfigurationUserStoreDatabaseSettings>(Configuration.GetSection(nameof(CardsConfigurationUserStoreDatabaseSettings)));
            services.AddSingleton<ICardsConfigurationUserStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CardsConfigurationUserStoreDatabaseSettings>>().Value);
        }
        
        private static void SwaggerService(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CanardEcarlate.Api", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private static void AutoMapperService(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private static void AuthenticationAuthorisationService(IServiceCollection services)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = "https://canardecarlate.fr",
                ValidAudience = "https://canardecarlate.fr",
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ")),
                ClockSkew = TimeSpan.Zero // remove delay of token when expire
            };

            services
                .AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(cfg => { cfg.TokenValidationParameters = tokenValidationParameters; });

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("player", policy => policy.RequireClaim("type", "player"));
                cfg.AddPolicy("ClearanceLevel1", policy => policy.RequireClaim("ClearanceLevel", "1"));
            });
        }
    }
}