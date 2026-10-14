# RaceDay.Api

This A .NET 8 Web API for managing race events, categories and enrolments. This repository contains the API project and automated tests.
It is Part 2 of the RaceDay API project.

## Key features
- Event and category management
- Participant enrolment workflows
- Authentication and role-based authorization
- EF Core for data access
- Automated tests with xUnit and integration tests using an in-memory database

## Tech stack
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- xUnit, FluentAssertions for tests

## Prerequisites
- .NET 8 SDK
- Visual Studio 2022/2026 or VS Code (with C# extension)

## Getting started (local)
1. Clone the repo
   git clone https://github.com/uChusta/Raceday.Api-Personal.git
   cd Raceday.Api

2. Restore and build
   dotnet restore
   dotnet build Raceday.Api.slnx

3. Configure
   - Copy `appsettings.Development.json` or `appsettings.json` as needed.
   - Set environment variables (examples):
     - ASPNETCORE_ENVIRONMENT=Development
     - ConnectionStrings__DefaultConnection="Server=...;Database=...;Trusted_Connection=True;"

4. Run
   - From solution root:
     dotnet run --project <path-to-api-csproj>
   - Or open the solution in Visual Studio and run via IIS Express / Kestrel.

## Running tests
- Run all tests:
  dotnet test

- Integration tests use an in-memory database (no external DB required).

## Database / Migrations
- If using a relational DB, use EF Core tools:
  dotnet tool install --global dotnet-ef
  dotnet ef migrations add InitialCreate --project <ApiProjectPath> --startup-project <ApiProjectPath>
  dotnet ef database update --project <ApiProjectPath> --startup-project <ApiProjectPath>

- In tests, an InMemory provider is used to isolate tests from production databases.

## API examples
- Base URL (development): https://localhost:5001
- Example: Register a user
  POST /api/auth/register
  Body: { "fullname": "...", "email": "...", "user_password": "..." }

(Replace with real endpoints and contract examples from RaceDay.Contracts.)

## Project structure (suggested)
- /src/Raceday.Api — Web API project
- /tests/RaceDay.Tests — Integration and unit tests
- /docs — Design docs / API contracts (optional)

## Contributing
1. Fork the repo
2. Create a feature branch: git checkout -b feat/your-feature
3. Make changes and add tests
4. Run tests locally: dotnet test
5. Create a pull request with a clear description of changes

## CI/CD
- Add pipeline to run:
  - dotnet restore
  - dotnet build
  - dotnet test

## License
Specify your license here (e.g., MIT). Replace this line with the actual license file reference.

## Maintainers / Contact
- Project: RaceDay.Api
- Repo: https://github.com/uChusta/Raceday.Api-Personal
- Contact: <maintainer-email-or-username>
