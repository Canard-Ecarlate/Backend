using CanardEcarlate.Application;
using CanardEcarlate.Infrastructure;
using CanardEcarlate.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
