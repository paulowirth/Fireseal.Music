# Fireseal Music API

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

Sample .NET 10 API for managing albums and tracks to demonstrate Clean Architecture, integration testing, and modern backend design.

## Business Capabilities
This simple music API allows authenticated users to manage music albums and tracks. It supports creating, listing, updating, and deleting albums and tracks, with all operations protected by JWT authentication. This API is suitable for music catalog management, digital music libraries, or as a backend for music-related applications.

## Features
- Clean architecture (Domain, Application, Infrastructure, Presentation, API)
- SQLite (local) database with Entity Framework Core
- JWT authentication (login and protected endpoints)
- Global exception handling middleware
- Unit and integration tests (with in-memory database for isolation)

## Endpoints Overview

| Endpoint                           | Method | Description                        | Auth Required |
|------------------------------------|--------|------------------------------------|--------------|
| `/authentication/token`            | POST   | Obtain JWT token                   | No           |
| `/fireseal/albums`                 | GET    | List all albums                    | Yes          |
| `/fireseal/albums`                 | POST   | Create a new album                 | Yes          |
| `/fireseal/albums/{id}`            | GET    | Get album by ID                    | Yes          |
| `/fireseal/albums/{id}`            | PUT    | Update album                       | Yes          |
| `/fireseal/albums/{id}`            | DELETE | Delete album                       | Yes          |
| `/fireseal/tracks`                 | GET    | List all tracks                    | Yes          |
| `/fireseal/tracks`                 | POST   | Create a new track                 | Yes          |
| `/fireseal/tracks/{id}`            | GET    | Get track by ID                    | Yes          |
| `/fireseal/tracks/{id}`            | PUT    | Update track                       | Yes          |
| `/fireseal/tracks/{id}`            | DELETE | Delete track                       | Yes          |

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

**GET /fireseal/albums**

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

**POST /fireseal/albums**

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

**GET /fireseal/albums/{id}**

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

**PUT /fireseal/albums/{id}**

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

**DELETE /fireseal/albums/{id}**

_Response:_
HTTP 200 OK

### Tracks

**GET /fireseal/tracks**

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

**POST /fireseal/tracks**

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
```json
{
  "id": "track-guid-here"
}
```

**GET /fireseal/tracks/{id}**

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

**PUT /fireseal/tracks/{id}**

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

**DELETE /fireseal/tracks/{id}**

_Response:_
HTTP 200 OK

## Project Structure
```
Fireseal.Music.sln
src/
  Fireseal.Music.Api/           # API entrypoint and configuration
  Fireseal.Music.Application/   # Application services, DTOs, business logic
  Fireseal.Music.Domain/        # Domain entities and abstractions
  Fireseal.Music.Infrastructure/# Persistence, context, repository implementations
  Fireseal.Music.Presentation/  # Controllers, middlewares, validators

test/
  Fireseal.Music.Tests.Unit/        # Unit tests
  Fireseal.Music.Tests.Integration/ # Integration tests (in-memory DB)
```

## Getting Started

### Prerequisites
- [.NET 10+ SDK](https://dotnet.microsoft.com/download)
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
   dotnet run --project src/Fireseal.Music.Api
   ```
4. **Access Swagger UI:**
   Open [https://localhost:{port}/swagger](https://localhost:5001/swagger) in your browser (the application should redirect automatically).

### Authentication
- Use the `/authentication/token` endpoint to obtain a JWT token (default user: `admin` / password: `admin`).
- Use the "Authorize" button in Swagger UI to authenticate and access protected endpoints.

### Running Tests
- **Unit tests:**
  ```sh
  dotnet test test/Fireseal.Musice.Tests.Unit
  ```
- **Integration tests:**
  ```sh
  dotnet test test/Fireseal.Music.Tests.Integration
  ```

Integration tests use an in-memory SQLite database and do not affect your local `fireseal.db` file.

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
