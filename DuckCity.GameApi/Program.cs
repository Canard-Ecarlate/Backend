using System.Reflection;
using System.Text;
using AutoMapper;
using DuckCity.Application.Services;
using DuckCity.Application.Services.Interfaces;
using DuckCity.GameApi;
using DuckCity.GameApi.Hub;
using DuckCity.GameApi.Mappings;
using DuckCity.Infrastructure;
using DuckCity.Infrastructure.Repositories;
using DuckCity.Infrastructure.Repositories.CacheImpl;
using DuckCity.Infrastructure.Repositories.MongoImpl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

InitDotEnv();


new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

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
AutoMapperServices();
SwaggerServices();
AuthenticationAuthorisationServices();
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
app.UseAuthentication();
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
    services.AddSingleton<IGameRepository, GameRepository>();
    services.AddSingleton<IAuthenticationService, AuthenticationService>();
    services.AddSingleton<IRoomService, RoomService>();
    
    // Mongo
    services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
    services.AddSingleton<IMongoDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
}
void AutoMapperServices()
{
    MapperConfiguration mapperConfig = new(mc => { mc.AddProfile(new MappingProfile()); });
    IMapper mapper = mapperConfig.CreateMapper();
    services.AddSingleton(mapper);
}
void AuthenticationAuthorisationServices()
{
    string strKey = Environment.GetEnvironmentVariable("SIGNATURE_KEY") ?? throw new InvalidOperationException();
    TokenValidationParameters tokenValidationParameters = new()
    {
        ValidIssuer = "https://canardecarlate.fr",
        ValidAudience = "https://canardecarlate.fr",
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strKey)),
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

void SwaggerServices()
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "DuckCity.GameApi", Version = "v1"});
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

void InitDotEnv() {
    string? root = Path.GetDirectoryName(typeof(Program).Assembly.Location);
    string dotenv = Path.Combine(root ?? throw new InvalidOperationException(), ".env");
    DotEnv.Load(dotenv);
}