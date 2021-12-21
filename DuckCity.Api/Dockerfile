FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DuckCity.Api/DuckCity.Api.csproj", "DuckCity.Api/"]
RUN dotnet restore "DuckCity.Api/DuckCity.Api.csproj"
COPY . .
WORKDIR "/src/DuckCity.Api"
RUN dotnet build "DuckCity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuckCity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuckCity.Api.dll"]
