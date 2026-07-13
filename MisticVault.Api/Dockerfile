FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj(s) and restore
COPY MisticVault.Api/MisticVault.Api.csproj MisticVault.Api/
COPY MisticVault.Application/MisticVault.Application.csproj MisticVault.Application/
COPY MisticVault.Infrastructure/MisticVault.Infrastructure.csproj MisticVault.Infrastructure/
COPY MisticVault.Core/MisticVault.Domain.csproj MisticVault.Core/
RUN dotnet restore MisticVault.Api/MisticVault.Api.csproj

# copy remaining sources and publish
COPY . .
WORKDIR /src/MisticVault.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MisticVault.Api.dll"]
