#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y sshpass


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y sshpass
WORKDIR /src
COPY ["CanardEcarlate.Api/CanardEcarlate.Api.csproj", "CanardEcarlate.Api/"]
RUN dotnet restore "CanardEcarlate.Api/CanardEcarlate.Api.csproj"
COPY . .
WORKDIR "/src/CanardEcarlate.Api"
RUN dotnet build "CanardEcarlate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y sshpass
RUN dotnet publish "CanardEcarlate.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y openssh-server
RUN apt-get install -y sshpass
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CanardEcarlate.Api.dll"]
COPY ["CanardEcarlate.Domain/CanardEcarlate.Domain.csproj", "CanardEcarlate.Domain/"]
COPY ["CanardEcarlate.Application/CanardEcarlate.Application.csproj", "CanardEcarlate.Application/"]
RUN dotnet restore "CanardEcarlate.Api/CanardEcarlate.Api.csproj"
COPY . .
WORKDIR "/src/CanardEcarlate.Api"
RUN dotnet build "CanardEcarlate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CanardEcarlate.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CanardEcarlate.Api.dll"]