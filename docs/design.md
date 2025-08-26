# Todo API - Design Document

## Overview
This document describes the design and architecture of the Todo API, a .NET 8 Web API service built following Test-Driven Development (TDD) principles and Clean Architecture patterns.

## Architecture

The solution follows Clean Architecture principles with clear separation of concerns:

```
TodoApi/
├── src/
│   ├── TodoApi.Api/           # Presentation Layer
│   ├── TodoApi.Core/          # Domain/Business Layer
│   └── TodoApi.Infrastructure/ # Data Access Layer
├── tests/
│   ├── TodoApi.UnitTests/     # Unit Tests
│   └── TodoApi.IntegrationTests/ # Integration Tests
```

### Layers

#### 1. Core Layer (Domain)
- **Entities**: User and TodoItem domain models
- **Interfaces**: Repository contracts
- **DTOs**: Data Transfer Objects for API contracts

#### 2. Infrastructure Layer
- **Data Access**: Entity Framework Core with DbContext
- **Repositories**: Implementation of domain interfaces
- **Database**: In-Memory database for development/testing

#### 3. API Layer (Presentation)
- **Controllers**: RESTful API endpoints
- **Dependency Injection**: Service registration
- **Middleware**: Error handling, Swagger documentation

## Domain Model

### User Entity
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<TodoItem> Todos { get; set; }
}
```

### TodoItem Entity
```csharp
public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}
```

## API Endpoints

### Users
- `POST /api/users` - Create a new user
- `GET /api/users/{id}/todos` - Get all todos for a specific user

### Todos
- `POST /api/todos` - Add a new todo item
- `PATCH /api/todos/{id}` - Mark a todo as complete/incomplete
- `DELETE /api/todos/{id}` - Delete a todo item
- `GET /api/todos/{id}` - Get a specific todo item

## Data Transfer Objects (DTOs)

### Request DTOs
- `CreateUserRequest`: Name, Email
- `CreateTodoRequest`: Title, Description, UserId
- `UpdateTodoRequest`: IsCompleted

### Response DTOs
- `UserResponse`: Id, Name, Email, CreatedAt
- `TodoResponse`: Id, Title, Description, IsCompleted, CreatedAt, CompletedAt, UserId

## Error Handling

The API returns appropriate HTTP status codes:
- `200 OK` - Successful GET/PATCH operations
- `201 Created` - Successful POST operations
- `204 No Content` - Successful DELETE operations
- `400 Bad Request` - Invalid request data
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource conflicts (e.g., duplicate email)

## Testing Strategy

### Test-Driven Development (TDD)
The application was built following TDD principles:
1. Write failing tests first
2. Write minimal code to make tests pass
3. Refactor while keeping tests green

### Test Coverage
- **Unit Tests**: 14 tests covering entities and repositories
- **Integration Tests**: 8 tests covering all API endpoints
- **Test Isolation**: Each test uses isolated in-memory database

### Test Categories
1. **Entity Tests**: Verify domain model behavior
2. **Repository Tests**: Verify data access logic
3. **Integration Tests**: End-to-end API testing

## Technology Stack

- **.NET 8**: Latest LTS version
- **ASP.NET Core Web API**: RESTful API framework
- **Entity Framework Core**: ORM with In-Memory provider
- **xUnit**: Testing framework
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing
- **Swagger/OpenAPI**: API documentation

## Development Principles

### Clean Architecture
- Dependency Inversion: Core doesn't depend on external concerns
- Separation of Concerns: Each layer has a single responsibility
- Testability: Easy to unit test business logic

### SOLID Principles
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Open for extension, closed for modification
- **Liskov Substitution**: Implementations are substitutable
- **Interface Segregation**: Focused, cohesive interfaces
- **Dependency Inversion**: Depend on abstractions, not concretions

### Repository Pattern
- Abstracts data access layer
- Enables easy unit testing
- Provides clean separation between domain and data access

## Security Considerations

- **Input Validation**: DTOs with data annotations
- **SQL Injection**: Entity Framework parameterized queries
- **Email Uniqueness**: Database unique constraint
- **GUID IDs**: Non-predictable identifiers

## Performance Considerations

- **In-Memory Database**: Fast for development/testing
- **Async/Await**: Non-blocking I/O operations
- **Entity Framework**: Optimized queries with Include()
- **Minimal APIs**: Efficient request processing

## Future Enhancements

- **Authentication/Authorization**: JWT tokens, user roles
- **Persistent Database**: SQL Server, PostgreSQL
- **Caching**: Redis, In-Memory caching
- **Logging**: Structured logging with Serilog
- **Validation**: FluentValidation library
- **API Versioning**: Support multiple API versions
- **Rate Limiting**: Prevent API abuse
- **Health Checks**: Monitor application health