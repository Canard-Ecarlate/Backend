FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y sshpass

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y sshpass
WORKDIR /src
COPY ["DuckCity.Api/DuckCity.Api.csproj", "DuckCity.Api/"]
RUN dotnet restore "DuckCity.Api/DuckCity.Api.csproj"
COPY . .
WORKDIR "/src/DuckCity.Api"
RUN dotnet build "DuckCity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y sshpass
RUN dotnet publish "DuckCity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y sshpass
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuckCity.Api.dll"]
COPY ["DuckCity.Domain/DuckCity.Domain.csproj", "DuckCity.Domain/"]
COPY ["DuckCity.Application/DuckCity.Application.csproj", "DuckCity.Application/"]
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