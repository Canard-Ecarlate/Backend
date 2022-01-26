using DuckCity.Application.Services;
using DuckCity.Application.Services.Interfaces;
using DuckCity.GameApi.Hub;
using DuckCity.Infrastructure;
using DuckCity.Infrastructure.Repositories;
using DuckCity.Infrastructure.Repositories.Interfaces;
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
    services.AddSingleton<IUserRepository, UserRepository>();
    services.AddSingleton<IRoomRepository, RoomRepository>();
    services.AddSingleton<IAuthenticationService, AuthenticationService>();
    services.AddSingleton<IRoomService, RoomService>();
    
    // Mongo
    services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
    services.AddSingleton<IMongoDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
}