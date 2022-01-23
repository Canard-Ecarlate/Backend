FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update && apt-get install -y --no-install-recommends openssh-server=latest && apt-get install -y --no-install-recommends sshpass=latest && apt-get clean \ && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update && apt-get install -y --no-install-recommends openssh-server=latest && apt-get install -y --no-install-recommends sshpass=latest && apt-get clean \ && rm -rf /var/lib/apt/lists/*
WORKDIR /src
COPY ["DuckCity.Api/DuckCity.Api.csproj", "DuckCity.Api/"]
RUN dotnet restore "DuckCity.Api/DuckCity.Api.csproj"
COPY . .
WORKDIR "/src/DuckCity.Api"
RUN dotnet build "DuckCity.Api.csproj" -c Release -o /app/build

FROM build AS publishone
RUN apt-get update && apt-get install -y --no-install-recommends openssh-server=latest && apt-get install -y --no-install-recommends sshpass=latest && apt-get clean \ && rm -rf /var/lib/apt/lists/* && dotnet publish "DuckCity.Api.csproj" -c Release -o /app/publish

FROM base AS finalone
WORKDIR /app
RUN apt-get update && apt-get install -y --no-install-recommends openssh-server=latest && apt-get install -y --no-install-recommends sshpass=latest && apt-get clean \ && rm -rf /var/lib/apt/lists/*
COPY --from=publishone /app/publish .
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
