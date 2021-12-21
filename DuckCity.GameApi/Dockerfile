FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DuckCity.GameApi/DuckCity.GameApi.csproj", "DuckCity.GameApi/"]
RUN dotnet restore "DuckCity.GameApi/DuckCity.GameApi.csproj"
COPY . .
WORKDIR "/src/DuckCity.GameApi"
RUN dotnet build "DuckCity.GameApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuckCity.GameApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuckCity.GameApi.dll"]
