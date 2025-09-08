# Abstra Challenge

## Table of Contents
- [Business Capabilities](#business-capabilities)
- [Features](#features)
- [Endpoints Overview](#endpoints-overview)
- [Endpoint Examples](#endpoint-examples)
  - [Authentication](#authentication)
  - [Albums](#albums)
  - [Tracks](#tracks)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Running the API](#running-the-api)
  - [Authentication](#authentication-1)
  - [Running Tests](#running-tests)
- [Architecture](#architecture)
- [Notes](#notes)
- [License](#license)

A simple, layered .NET API for managing albums and tracks, built with best practices for architecture, testing, and maintainability.

## Business Capabilities
The Abstra Challenge API allows authenticated users to manage music albums and tracks. It supports creating, listing, updating, and deleting albums and tracks, with all operations protected by JWT authentication. This API is suitable for music catalog management, digital music libraries, or as a backend for music-related applications.

## Features
- Clean architecture (Domain, Application, Infrastructure, Presentation, API)
- SQLite (local) database with Entity Framework Core
- JWT authentication (login and protected endpoints)
- Global exception handling middleware
- Unit and integration tests (with in-memory database for isolation)

## Endpoints Overview

| Endpoint                        | Method | Description                        | Auth Required |
|----------------------------------|--------|------------------------------------|--------------|
| `/authentication/token`          | POST   | Obtain JWT token                   | No           |
| `/abstra/albums`                 | GET    | List all albums                    | Yes          |
| `/abstra/albums`                 | POST   | Create a new album                 | Yes          |
| `/abstra/albums/{id}`            | GET    | Get album by ID                    | Yes          |
| `/abstra/albums/{id}`            | PUT    | Update album                       | Yes          |
| `/abstra/albums/{id}`            | DELETE | Delete album                       | Yes          |
| `/abstra/tracks`                 | GET    | List all tracks                    | Yes          |
| `/abstra/tracks`                 | POST   | Create a new track                 | Yes          |
| `/abstra/tracks/{id}`            | GET    | Get track by ID                    | Yes          |
| `/abstra/tracks/{id}`            | PUT    | Update track                       | Yes          |
| `/abstra/tracks/{id}`            | DELETE | Delete track                       | Yes          |

## Endpoint Examples

### Authentication

**POST /authentication/token**

_Request:_
```json
{
  "username": "admin",
  "password": "admin"
}
```
_Response:_
```json
{
  "token": "<jwt-token>"
}
```

### Albums

**GET /abstra/albums**

_Response:_
```json
[
  {
    "id": "album-guid-here",
    "title": "My Album",
    "artist": "The Band",
    "releaseDate": "2025-01-01",
    "tracks": [
      { "id": "track-guid-here", "title": "Track 1", "duration": "00:03:30", "isrc": "ISRC12345678" }
    ]
  }
]
```

**POST /abstra/albums**

_Request:_
```json
{
  "title": "My Album",
  "artist": "The Band",
  "releaseDate": "2025-01-01",
  "tracks": [
    { "title": "Track 1", "duration": "00:03:30", "isrc": "ISRC12345678" }
  ]
}
```
_Response:_
```json
{
  "id": "album-guid-here"
}
```

**GET /abstra/albums/{id}**

_Response:_
```json
{
  "id": "album-guid-here",
  "title": "My Album",
  "artist": "The Band",
  "releaseDate": "2025-01-01",
  "tracks": [
    { "id": "track-guid-here", "title": "Track 1", "duration": "00:03:30", "isrc": "ISRC12345678" }
  ]
}
```

**PUT /abstra/albums/{id}**

_Request:_
```json
{
  "title": "Updated Album",
  "artist": "Updated Artist",
  "releaseDate": "2026-01-01"
}
```
_Response:_
HTTP 204 No Content

**DELETE /abstra/albums/{id}**

_Response:_
HTTP 200 OK

### Tracks

**GET /abstra/tracks**

_Response:_
```json
[
  {
    "id": "track-guid-here",
    "title": "Track 1",
    "duration": "00:03:30",
    "isrc": "ISRC12345678",
    "albumId": "album-guid-here"
  }
]
```

**POST /abstra/tracks**

_Request:_
```json
{
  "title": "Track 1",
  "duration": "00:03:30",
  "isrc": "ISRC12345678",
  "albumId": "album-guid-here"
}
```
_Response:_
{
  "id": "track-guid-here"
}
```

**GET /abstra/tracks/{id}**

_Response:_
```json
{
  "id": "track-guid-here",
  "title": "Track 1",
  "duration": "00:03:30",
  "isrc": "ISRC12345678",
  "albumId": "album-guid-here"
}
```

**PUT /abstra/tracks/{id}**

_Request:_
```json
{
  "title": "Updated Track",
  "duration": "00:04:00",
  "isrc": "ISRC87654321",
  "albumId": "album-guid-here"
}
```
_Response:_
HTTP 204 No Content

**DELETE /abstra/tracks/{id}**

_Response:_
HTTP 200 OK

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
