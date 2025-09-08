# Abstra Challenge

A simple, layered .NET API for managing albums and tracks, built with best practices for architecture, testing, and maintainability.

## Features
- Clean architecture (Domain, Application, Infrastructure, Presentation, API)
- SQLite (local) database with Entity Framework Core
- JWT authentication (login and protected endpoints)
- Global exception handling middleware
- Unit and integration tests (with in-memory database for isolation)

## Project Structure
```
Abstra.Challenge.sln
src/
  Abstra.Challenge.Api/           # API entrypoint and configuration
  Abstra.Challenge.Application/   # Application services, DTOs, business logic
  Abstra.Challenge.Domain/        # Domain entities and abstractions
  Abstra.Challenge.Infrastructure/# Persistence, context, repository implementations
  Abstra.Challenge.Presentation/  # Controllers, middlewares, validators

test/
  Abstra.Challenge.Tests.Unit/        # Unit tests
  Abstra.Challenge.Tests.Integration/ # Integration tests (in-memory DB)
```

## Getting Started

### Prerequisites
- [.NET 9+ SDK](https://dotnet.microsoft.com/download)
- (Optional) [SQLite CLI](https://www.sqlite.org/download.html) for inspecting the database

### Running the API

1. **Restore dependencies:**
   ```sh
   dotnet restore
   ```
2. **Build the solution:**
   ```sh
   dotnet build
   ```
3. **Run the API:**
   ```sh
   dotnet run --project src/Abstra.Challenge.Api
   ```
4. **Access Swagger UI:**
   Open [https://localhost:{port}/swagger](https://localhost:5001/swagger) in your browser (the application should redirect automatically).

### Authentication
- Use the `/authentication/token` endpoint to obtain a JWT token (default user: `admin` / password: `admin`).
- Use the "Authorize" button in Swagger UI to authenticate and access protected endpoints.

### Running Tests
- **Unit tests:**
  ```sh
  dotnet test test/Abstra.Challenge.Tests.Unit
  ```
- **Integration tests:**
  ```sh
  dotnet test test/Abstra.Challenge.Tests.Integration
  ```

Integration tests use an in-memory SQLite database and do not affect your local `abstra.db` file.

## Key Endpoints

- `POST /authentication/token` — Obtain JWT token
- `GET /abstra/albums` — List all albums (protected)
- `POST /abstra/albums` — Create album (protected)
- `GET /abstra/albums/{id}` — Get album by ID (protected)
- `PUT /abstra/albums/{id}` — Update album (protected)
- `DELETE /abstra/albums/{id}` — Delete album (protected)
- `GET /abstra/tracks` — List all tracks (protected)
- ...and more

## Architecture
- **Domain:** Core entities and business rules
- **Application:** Services, DTOs, business logic
- **Infrastructure:** Persistence, database context, repository implementations
- **Presentation:** Controllers, middlewares, validators
- **API:** Startup, configuration, DI

## Notes
- The project is designed for simplicity and maintainability.
- All database files (`*.db`, `*.db-shm`, `*.db-wal`) are git-ignored.
- Integration tests are fully isolated and do not require a real database file.

## License
MIT (or specify your license here)

