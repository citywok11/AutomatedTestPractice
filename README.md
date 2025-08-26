# Todo API - Automated Test Practice

A .NET 8 Todo API service built with Clean Architecture and Test-Driven Development (TDD) principles for practicing automated testing.

## Project Structure

```
TodoApi/
├── src/
│   ├── TodoApi.Core/          # Domain entities and interfaces
│   ├── TodoApi.Infrastructure/ # EF Core, repositories, services
│   └── TodoApi.API/           # Controllers, DTOs, middleware
├── tests/
│   ├── TodoApi.UnitTests/     # Unit tests with Moq
│   └── TodoApi.IntegrationTests/ # Integration tests
├── TodoApi.sln                # Solution file
├── Dockerfile                 # Docker containerization
└── README.md                  # This file
```

## API Endpoints

The Todo API provides 5 main endpoints:

1. `POST /api/users` - Create a new user
2. `GET /api/users/{id}/todos` - Get all todos for a user
3. `POST /api/todos?userId={id}` - Add a new todo for a user
4. `PATCH /api/todos/{id}` - Mark a todo as complete
5. `DELETE /api/todos/{id}` - Delete a todo

## Technologies

- **.NET 8** - Framework
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM with In-Memory database
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Testing framework
- **Moq** - Mocking library
- **Docker** - Containerization

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker (optional, for containerization)

### Build and Run

```bash
# Build the solution
dotnet build

# Run the API
cd src/TodoApi.API
dotnet run

# Run tests
dotnet test
```

The API will be available at `http://localhost:5248` with Swagger UI at `http://localhost:5248/swagger`.

## Testing

The project includes comprehensive unit and integration tests:

- **Unit Tests**: Test business logic in isolation using Moq
- **Integration Tests**: Test API endpoints end-to-end using in-memory test server

## Architecture

The project follows Clean Architecture principles:

- **Core**: Contains domain entities and interfaces
- **Infrastructure**: Contains data access, repositories, and services
- **API**: Contains controllers, DTOs, and configuration

This separation ensures testability, maintainability, and independence from external frameworks.