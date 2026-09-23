EF Migrations and Update Instructions

1. Ensure your connection string is configured in EPareH60Store/appsettings.json under ConnectionStrings:DefaultConnection.

2. To create migrations locally (recommended approach):
   - dotnet ef migrations add InitialAssignmentEntities -p EPareH60Store -s EPareH60Store -o Migrations

3. To apply migrations to the database:
   - dotnet ef database update -p EPareH60Store -s EPareH60Store

Notes:
- The workspace includes hand-crafted migration files under EPareH60Store/Migrations. If you prefer to use the built-in tooling, delete or rename those files and run the commands above to scaffold a proper migration snapshot.
- Verify seeded product IDs (1..20) do not conflict with an existing database before running Update-Database on an instructor copy.
