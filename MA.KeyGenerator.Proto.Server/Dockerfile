#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MA.KeyGenerator.Proto.Server/MA.KeyGenerator.Proto.Server.csproj", "MA.KeyGenerator.Proto.Server/"]
ARG NUGET_PAT
RUN dotnet nuget add source "https://nuget.pkg.github.com/mat-docs/index.json" --name MA --username "docker" --password "$NUGET_PAT" --store-password-in-clear-text
RUN dotnet restore "./MA.KeyGenerator.Proto.Server/MA.KeyGenerator.Proto.Server.csproj" 
COPY . .
WORKDIR "/src/MA.KeyGenerator.Proto.Server"
RUN dotnet build "./MA.KeyGenerator.Proto.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MA.KeyGenerator.Proto.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MA.KeyGenerator.Proto.Server.dll"]