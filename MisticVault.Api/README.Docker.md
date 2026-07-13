Docker usage for MisticVault.Api

Quick start (developer):

1. Copy .env.example to .env and set SA_PASSWORD to a strong password.
2. Build and start containers:
   docker compose up -d --build
3. Apply EF migrations (if using migrations):
   docker compose run --rm api dotnet ef database update --no-build -p MisticVault.Api/MisticVault.Api.csproj
4. View logs:
   docker compose logs -f api

Stop and remove containers and volumes:
   docker compose down -v

Notes:
- For production, replace the DB container with a managed SQL instance and set ConnectionStrings__DefaultConnection accordingly.
- Use a secrets manager or docker secrets to store SA_PASSWORD in production.
