# .NET 8 Todo API - Automated Test Practice

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Project Overview

This is a .NET 8 Todo API service built with Clean Architecture and Test-Driven Development (TDD) principles. The project includes a complete implementation with 5 API endpoints, comprehensive unit and integration tests, and Docker containerization.

## Working Effectively

### Prerequisites and Environment Setup
- **.NET 8 SDK is pre-installed** (version 8.0.119)
- **Docker is available** (version 28.0.4)
- All required tools are already available in the environment

### Bootstrap, Build, and Test Commands

**CRITICAL TIMING AND TIMEOUT REQUIREMENTS:**
- **NEVER CANCEL** any build or test commands - they complete successfully but take time
- **Build timeout**: Set minimum 60 seconds (actual: ~13 seconds, but can vary)
- **Test timeout**: Set minimum 60 seconds (actual: ~10 seconds, but can vary)
- **NuGet restore timeout**: Set minimum 120 seconds (can take 30+ seconds)

```bash
# Navigate to repository root
cd /home/runner/work/AutomatedTestPractice/AutomatedTestPractice

# Build the solution - NEVER CANCEL, takes ~13 seconds
dotnet build
# Expected output: "Build succeeded" with 1 warning (safe to ignore)

# Run all tests - NEVER CANCEL, takes ~10 seconds
dotnet test
# Expected output: "Passed! - Failed: 0, Passed: 7, Skipped: 0"

# Run only unit tests
dotnet test tests/TodoApi.UnitTests/
# Expected: 4 tests pass in <1 second

# Run only integration tests  
dotnet test tests/TodoApi.IntegrationTests/
# Expected: 3 tests pass in <1 second
```

### Running the API

```bash
# Start the API server
cd src/TodoApi.API
dotnet run
# Expected output: "Now listening on: http://localhost:5248"
# Swagger UI available at: http://localhost:5248/swagger

# Stop with Ctrl+C
```

### Docker Commands

```bash
# Build Docker image - NEVER CANCEL, can take 5+ minutes
# NOTE: May fail due to network restrictions in CI/restricted environments
docker build -t todoapi .

# Run containerized API (if build succeeds)
docker run -p 8080:8080 todoapi
```

## Validation and Testing

### Manual API Validation
**ALWAYS** run these validation scenarios after making changes to verify all 5 endpoints work:

```bash
# Start the API first
cd src/TodoApi.API && dotnet run &
sleep 5  # Wait for startup

# 1. Create user
curl -X POST http://localhost:5248/api/users \
  -H "Content-Type: application/json" \
  -d '{"name": "Test User", "email": "test@example.com"}'
# Expected: JSON response with user id, name, email, and createdAt

# 2. Get user todos (empty initially)
curl http://localhost:5248/api/users/1/todos
# Expected: Empty array []

# 3. Add todo
curl -X POST "http://localhost:5248/api/todos?userId=1" \
  -H "Content-Type: application/json" \
  -d '{"title": "Test Todo", "description": "Test Description"}'
# Expected: JSON response with todo details, isCompleted: false

# 4. Mark todo complete
curl -X PATCH http://localhost:5248/api/todos/1 \
  -H "Content-Type: application/json" \
  -d '{"isCompleted": true}'
# Expected: JSON response with isCompleted: true

# 5. Delete todo
curl -X DELETE http://localhost:5248/api/todos/1
# Expected: HTTP 204 No Content

# Stop the API
pkill -f dotnet || true
```

### Required Validation Before Committing
1. **Always run** `dotnet build` - must succeed with 0 errors (1 warning is expected)
2. **Always run** `dotnet test` - all 7 tests must pass
3. **Always test** the 5 API endpoints manually as shown above
4. **Always verify** new features work end-to-end

## Project Structure and Key Files

### Solution Structure
```
TodoApi.sln                 # Main solution file
src/
  TodoApi.Core/            # Domain entities and interfaces
    Class1.cs              # User and Todo entities
    Interfaces.cs          # Repository and service interfaces
  TodoApi.Infrastructure/   # Data access and business logic
    Class1.cs              # EF Core DbContext
    Repositories.cs        # User and Todo repositories
    Services.cs            # TodoService business logic
  TodoApi.API/             # Web API controllers and configuration
    Controllers/           # API controllers
      UsersController.cs   # User management endpoints
      TodosController.cs   # Todo management endpoints
    DTOs.cs               # Data transfer objects
    Program.cs            # Application startup and DI configuration
tests/
  TodoApi.UnitTests/       # Unit tests with Moq
    UnitTest1.cs          # TodoService unit tests (4 tests)
  TodoApi.IntegrationTests/ # Integration tests
    UnitTest1.cs          # End-to-end API tests (3 tests)
```

### Key Technologies and Packages
- **EF Core In-Memory**: Used for data storage (no external database needed)
- **Moq 4.20.72**: Mocking framework for unit tests
- **Microsoft.AspNetCore.Mvc.Testing 8.0.8**: Integration testing framework
- **Swagger/OpenAPI**: Automatic API documentation

## Common Tasks and Troubleshooting

### Adding New Features
1. **Always start with tests** (TDD approach)
2. Add failing test first in appropriate test project
3. Implement minimal code to make test pass
4. Refactor while keeping tests green
5. Run full validation suite before committing

### Dependencies and Package Management
```bash
# Add new NuGet package (example)
dotnet add src/TodoApi.API/TodoApi.API.csproj package [PackageName]

# Restore packages if needed - NEVER CANCEL, timeout 120+ seconds
dotnet restore
```

### Database and EF Core
- **Uses In-Memory database** - no setup required
- **Database is recreated** for each test run
- **No migrations needed** - schema is code-first from entities
- All data is lost when application stops (by design)

### Build Issues
- **Single expected warning**: CS1998 in TodosController.cs GetTodo method (safe to ignore)
- **If build fails**: Check for syntax errors or missing dependencies
- **NuGet issues**: Run `dotnet restore` and wait for completion

### Test Issues
- **Integration tests require**: API project to be buildable
- **Test isolation**: Each test runs with fresh in-memory database
- **Network warnings in integration tests**: Expected and safe to ignore

## Architecture Notes

### Clean Architecture Layers
1. **Core (Domain)**: Entities and interfaces only, no dependencies
2. **Infrastructure**: Implements Core interfaces, depends on Core
3. **API**: Controllers and DTOs, depends on Core and Infrastructure

### Key Design Patterns
- **Repository Pattern**: Abstracts data access
- **Dependency Injection**: All dependencies injected via ASP.NET Core DI
- **CQRS-lite**: Services handle commands, repositories handle queries

### API Design
- **RESTful endpoints** following HTTP conventions
- **Proper status codes**: 200 OK, 201 Created, 204 No Content, 400 Bad Request, 404 Not Found
- **JSON request/response** format
- **Validation**: Basic validation in service layer

Always build and exercise your changes following the validation steps above to ensure they work correctly in this TDD-focused practice environment.