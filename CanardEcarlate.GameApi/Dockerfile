FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CanardEcarlate.GameApi/CanardEcarlate.GameApi.csproj", "CanardEcarlate.GameApi/"]
RUN dotnet restore "CanardEcarlate.GameApi/CanardEcarlate.GameApi.csproj"
COPY . .
WORKDIR "/src/CanardEcarlate.GameApi"
RUN dotnet build "CanardEcarlate.GameApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CanardEcarlate.GameApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CanardEcarlate.GameApi.dll"]
