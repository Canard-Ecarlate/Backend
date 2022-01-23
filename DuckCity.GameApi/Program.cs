using DuckCity.Application.Services;
using DuckCity.GameApi.Hub;
using DuckCity.Infrastructure.Repositories;
using DuckCity.Infrastructure.StoreDatabaseSettings;
using DuckCity.Infrastructure.StoreDatabaseSettings.Interfaces;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IServiceCollection services = builder.Services;

/*
 * Services
 */
Cors();
Singletons();
services.AddControllers();
MongoServices();
services.AddSignalR();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

/*
 * Build app
 */
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();
app.MapHub<DuckCityHub>("/gameId");
app.Run();


/*
 * Methods
 */
void Cors()
{
    services.AddCors(options => options
        .AddPolicy("CorsPolicy",
            policyBuilder => policyBuilder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()
                .AllowCredentials()));
}
void Singletons()
{
    services.AddSingleton<UserRepository>();
    services.AddSingleton<RoomRepository>();
    services.AddSingleton<AuthenticationService>();
    services.AddSingleton<RoomService>();
}
void MongoServices()
{
    services.Configure<UserStoreDatabaseSettings>(configuration.GetSection(nameof(UserStoreDatabaseSettings)));
    services.AddSingleton<IUserStoreDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<UserStoreDatabaseSettings>>().Value);

    services.Configure<RoomStoreDatabaseSettings>(configuration.GetSection(nameof(RoomStoreDatabaseSettings)));
    services.AddSingleton<IRoomStoreDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<RoomStoreDatabaseSettings>>().Value);

    services.Configure<UserStatisticsStoreDatabaseSettings>(configuration.GetSection(nameof(UserStatisticsStoreDatabaseSettings)));
    services.AddSingleton<IUserStatisticsStoreDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<UserStatisticsStoreDatabaseSettings>>().Value);
            
    services.Configure<GlobalStatisticsStoreDatabaseSettings>(configuration.GetSection(nameof(GlobalStatisticsStoreDatabaseSettings)));
    services.AddSingleton<IGlobalStatisticsStoreDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<GlobalStatisticsStoreDatabaseSettings>>().Value);
            
    services.Configure<CardsConfigurationUserStoreDatabaseSettings>(configuration.GetSection(nameof(CardsConfigurationUserStoreDatabaseSettings)));
    services.AddSingleton<ICardsConfigurationUserStoreDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<CardsConfigurationUserStoreDatabaseSettings>>().Value);
}
