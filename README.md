# .NET 8 Todo API Service

A complete .NET 8 Web API service for managing users and todo items, built following Test-Driven Development (TDD) principles and Clean Architecture patterns.

## Features

- ✅ **Complete RESTful API** with 5 endpoints
- ✅ **Clean Architecture** with proper separation of concerns
- ✅ **Test-Driven Development** with comprehensive test coverage
- ✅ **Selenium Automation Tests** for end-to-end browser testing
- ✅ **Interactive Database Viewer** web interface
- ✅ **Entity Framework Core** with In-Memory database
- ✅ **Swagger/OpenAPI** documentation
- ✅ **Dependency Injection** and Repository pattern
- ✅ **Docker** containerization support
- ✅ **GitHub Codespaces** compatibility
- ✅ **Input validation** and error handling

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/users` | Create a new user |
| GET | `/api/users/{id}/todos` | Get all todos for a user |
| POST | `/api/todos` | Create a new todo item |
| PATCH | `/api/todos/{id}` | Mark todo as complete/incomplete |
| DELETE | `/api/todos/{id}` | Delete a todo item |

## Project Structure

```
TodoApi/
├── src/
│   ├── TodoApi.Api/           # Web API controllers and configuration
│   ├── TodoApi.Core/          # Domain entities, DTOs, and interfaces
│   └── TodoApi.Infrastructure/ # Data access with Entity Framework
├── tests/
│   ├── TodoApi.UnitTests/     # Unit tests (14 tests)
│   └── TodoApi.IntegrationTests/ # Integration tests (8 tests)
├── selenium-tests/            # Selenium WebDriver automation tests
│   ├── package.json           # Node.js dependencies
│   ├── test-todoapi.js        # Main Selenium test suite
│   └── node_modules/          # NPM packages
├── docs/
│   ├── design.md              # Architecture and design documentation
│   └── api.md                 # API documentation
├── Dockerfile                 # Container configuration
└── README.md                  # This file
```

## Technologies Used

- **.NET 8** - Latest LTS framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core** - ORM with In-Memory provider
- **xUnit** - Testing framework
- **Selenium WebDriver** - Browser automation testing
- **Node.js** - JavaScript runtime for Selenium tests
- **Swagger/OpenAPI** - API documentation
- **Docker** - Containerization

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (optional)

### Running Locally

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd TodoApi
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the tests**
   ```bash
   dotnet test
   ```

4. **Start the API**
   ```bash
   cd src/TodoApi.Api
   dotnet run
   ```

5. **Access the API**
   - API Base URL: `http://localhost:5119/api`
   - Swagger UI: `http://localhost:5119/swagger`

### Using Docker

1. **Build the Docker image**
   ```bash
   docker build -t todoapi .
   ```

2. **Run the container**
   ```bash
   docker run -p 8080:8080 todoapi
   ```

3. **Access the API**
   - API Base URL: `http://localhost:8080/api`
   - Swagger UI: `http://localhost:8080/swagger`

## Usage Examples

### Create a User
```bash
curl -X POST http://localhost:5119/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "email": "john@example.com"
  }'
```

### Create a Todo
```bash
curl -X POST http://localhost:5119/api/todos \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Learn .NET 8",
    "description": "Study new features",
    "userId": "user-id-from-above"
  }'
```

### Get User's Todos
```bash
curl http://localhost:5119/api/users/{user-id}/todos
```

### Mark Todo as Complete
```bash
curl -X PATCH http://localhost:5119/api/todos/{todo-id} \
  -H "Content-Type: application/json" \
  -d '{"isCompleted": true}'
```

### Delete a Todo
```bash
curl -X DELETE http://localhost:5119/api/todos/{todo-id}
```

## Selenium Automation Tests

This project includes comprehensive **Selenium WebDriver** automation tests to verify the API functionality through browser-based testing.

### Test Setup

The Selenium tests are located in the `/selenium-tests/` directory and use:
- **Node.js** with **Selenium WebDriver** 4.20.0
- **Chrome** browser in headless mode
- **JavaScript** test scripts with async/await patterns

### Prerequisites for Selenium Tests

1. **Node.js** (v14 or higher)
2. **Chrome browser** (automatically detected)
3. **ChromeDriver** (automatically managed by Selenium Manager)

### Running Selenium Tests

1. **Navigate to the selenium tests directory**
   ```bash
   cd selenium-tests
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Start the TodoApi server** (in another terminal)
   ```bash
   cd src/TodoApi.Api
   dotnet run
   ```

4. **Run the Selenium tests**
   ```bash
   npm test
   ```

### Test Features

The Selenium automation tests verify:

- ✅ **API Connectivity**: Tests server response and availability
- ✅ **User Creation**: Automated user registration through API calls
- ✅ **Todo Management**: Create, read, update, and delete operations
- ✅ **Database Viewer**: Interactive web interface for viewing/managing todos
- ✅ **Cross-browser Compatibility**: Headless Chrome testing
- ✅ **Error Handling**: Validation of error responses and edge cases

### Database Viewer

The project includes a web-based database viewer accessible at:
- **Local**: `http://localhost:5119/`
- **Codespaces**: `https://your-codespace-url.app.github.dev/`

The viewer provides:
- Real-time todo list display
- Add/edit/delete todo functionality
- User management interface
- Interactive API testing
- Responsive design for mobile and desktop

### Test Configuration

```javascript
// selenium-tests/test-todoapi.js
const options = new chrome.Options();
options.addArguments('--headless');  // Run in background
options.addArguments('--no-sandbox'); // Required for containers
options.addArguments('--disable-dev-shm-usage'); // Stability
```

### GitHub Codespaces Support

The Selenium tests are fully compatible with **GitHub Codespaces**:
- Automatic Chrome/ChromeDriver installation
- Container-optimized browser flags
- Dynamic URL detection for Codespaces environments
- Public port accessibility for external testing

## Development

### Project Architecture

This project follows **Clean Architecture** principles:

- **Core Layer**: Domain entities, DTOs, and repository interfaces
- **Infrastructure Layer**: Data access implementation with Entity Framework
- **API Layer**: Controllers, dependency injection, and middleware

### Test-Driven Development

The project was built using TDD methodology:

1. **Write failing tests first**
2. **Write minimal code to make tests pass**
3. **Refactor while keeping tests green**

**Test Coverage:**
- Unit Tests: 14 tests covering entities and repositories
- Integration Tests: 8 tests covering all API endpoints
- All tests use in-memory database for isolation

### Running Tests

```bash
# Run all tests
dotnet test

# Run only unit tests
dotnet test tests/TodoApi.UnitTests/

# Run only integration tests
dotnet test tests/TodoApi.IntegrationTests/

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Code Quality

The codebase follows:
- **SOLID principles**
- **Repository pattern** for data access
- **Dependency injection** for loose coupling
- **Input validation** with data annotations
- **Async/await** for non-blocking operations
- **Proper error handling** with meaningful HTTP status codes

## API Documentation

Detailed API documentation is available in:
- [API Documentation](docs/api.md) - Complete endpoint reference
- [Design Documentation](docs/design.md) - Architecture and design decisions

When running the application, interactive documentation is available at `/swagger`.

## Configuration

The application uses the following configuration:

### Database
- **Development**: In-Memory database (no persistence)
- **Testing**: Isolated in-memory databases per test

### Logging
- Built-in ASP.NET Core logging
- Console and Debug providers in development

### CORS
- Currently allows all origins (development only)
- Configure restrictive CORS policy for production

## Deployment

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Environment name | `Development` |
| `ASPNETCORE_URLS` | Listening URLs | `http://localhost:5119` |

### Production Considerations

Before deploying to production:

1. **Database**: Replace in-memory database with persistent storage (SQL Server, PostgreSQL)
2. **Authentication**: Implement JWT-based authentication
3. **CORS**: Configure restrictive CORS policy
4. **Rate Limiting**: Add rate limiting middleware
5. **Logging**: Implement structured logging (Serilog)
6. **Health Checks**: Add health check endpoints
7. **Monitoring**: Add application monitoring and metrics

## Contributing

1. Fork the repository
2. Create a feature branch
3. Write tests for new functionality
4. Implement the feature
5. Ensure all tests pass
6. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For questions or issues, please:
1. Check the [API Documentation](docs/api.md)
2. Review the [Design Documentation](docs/design.md)
3. Run the test suite to verify functionality
4. Open an issue in the repository

---

**Built with ❤️ using .NET 8 and Clean Architecture**