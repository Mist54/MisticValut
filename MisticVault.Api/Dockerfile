FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. Copy ALL csproj files exactly matching your solution structure
COPY ["MisticVault.Api/MisticVault.Api.csproj", "MisticVault.Api/"]
COPY ["MisticVault.Application/MisticVault.Application.csproj", "MisticVault.Application/"]
COPY ["MisticVault.Infrastructure/MisticVault.Infrastructure.csproj", "MisticVault.Infrastructure/"]
# Double check if this folder is Core or Domain in your actual repository!
COPY ["MisticVault.Core/MisticVault.Domain.csproj", "MisticVault.Core/"]

# 2. Restore packages for the main entry point (this restores for dependencies too)
RUN dotnet restore "MisticVault.Api/MisticVault.Api.csproj"

# 3. Copy the rest of the source code ONLY AFTER restoring
COPY . .
WORKDIR "/src/MisticVault.Api"

# 4. Build and publish
RUN dotnet publish "MisticVault.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 5. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Note: ASP.NET Core 8.0+ (including 10.0) defaults to port 8080, not 80.
# It's safer to specify 8080, or keep your ENV if you explicitly want port 80.
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MisticVault.Api.dll"]