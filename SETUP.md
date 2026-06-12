# Setup and Secrets

Steps to configure development environment:

1. Copy appsettings.json and fill secrets:
   - Jwt:PrivateKey -> set a strong symmetric key (or RSA key pair and adapt JwtService)
   - Authentication SSO client ids/secrets for Google, Microsoft, GitHub
   - ConnectionStrings:Default -> SQLite path for dev or SQL Server for prod

2. Run migrations (EF Core) or apply Flyway scripts:
   - Using EF Core: run script scripts/generate-migrations.ps1 to create migrations and SQL for Flyway
     - powershell -ExecutionPolicy Bypass -File scripts/generate-migrations.ps1
   - Or apply Flyway scripts in /flyway using the Flyway CLI

3. Run the API:
   dotnet run --project src/Api

4. Frontend:
   cd frontend
   npm ci
   npm run dev

5. CI/CD:
   - Add secrets to GitHub Actions: SSH_PRIVATE_KEY, OCI_USER, OCI_HOST, etc.

Security notes:
- Never commit real secrets to git. Use user-secrets or environment variables.
- For production, use RSA keys and rotate them periodically.
