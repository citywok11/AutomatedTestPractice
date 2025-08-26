# Todo API Documentation

A RESTful API service for managing users and their todo items, built with .NET 8 and following Clean Architecture principles.

## Base URL
```
http://localhost:5119/api
```

## Authentication
Currently, no authentication is required. All endpoints are publicly accessible.

## Content Types
- Request: `application/json`
- Response: `application/json`

## API Endpoints

### Users

#### Create User
Creates a new user account.

**Endpoint:** `POST /users`

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john.doe@example.com"
}
```

**Response:** `201 Created`
```json
{
  "id": "a3bab04f-c7da-4227-9a4f-2192c6223164",
  "name": "John Doe", 
  "email": "john.doe@example.com",
  "createdAt": "2023-12-01T10:30:00Z"
}
```

**Error Responses:**
- `400 Bad Request` - Invalid request data
- `409 Conflict` - Email already exists

#### Get User's Todos
Retrieves all todo items for a specific user.

**Endpoint:** `GET /users/{id}/todos`

**Path Parameters:**
- `id` (uuid) - User ID

**Response:** `200 OK`
```json
[
  {
    "id": "b4cbc05f-d8eb-5338-af5f-3293d7334275",
    "title": "Complete project",
    "description": "Finish the todo API implementation",
    "isCompleted": false,
    "createdAt": "2023-12-01T10:35:00Z",
    "completedAt": null,
    "userId": "a3bab04f-c7da-4227-9a4f-2192c6223164"
  }
]
```

**Error Responses:**
- `404 Not Found` - User not found

### Todos

#### Create Todo
Creates a new todo item for a user.

**Endpoint:** `POST /todos`

**Request Body:**
```json
{
  "title": "Learn .NET 8",
  "description": "Study new features in .NET 8",
  "userId": "a3bab04f-c7da-4227-9a4f-2192c6223164"
}
```

**Response:** `201 Created`
```json
{
  "id": "c5dcd16f-e9fc-6449-bg6f-4394e8445386",
  "title": "Learn .NET 8",
  "description": "Study new features in .NET 8", 
  "isCompleted": false,
  "createdAt": "2023-12-01T11:00:00Z",
  "completedAt": null,
  "userId": "a3bab04f-c7da-4227-9a4f-2192c6223164"
}
```

**Error Responses:**
- `400 Bad Request` - Invalid request data or user doesn't exist

#### Get Todo
Retrieves a specific todo item.

**Endpoint:** `GET /todos/{id}`

**Path Parameters:**
- `id` (uuid) - Todo ID

**Response:** `200 OK`
```json
{
  "id": "c5dcd16f-e9fc-6449-bg6f-4394e8445386",
  "title": "Learn .NET 8",
  "description": "Study new features in .NET 8",
  "isCompleted": false,
  "createdAt": "2023-12-01T11:00:00Z",
  "completedAt": null,
  "userId": "a3bab04f-c7da-4227-9a4f-2192c6223164"
}
```

**Error Responses:**
- `404 Not Found` - Todo not found

#### Update Todo
Updates the completion status of a todo item.

**Endpoint:** `PATCH /todos/{id}`

**Path Parameters:**
- `id` (uuid) - Todo ID

**Request Body:**
```json
{
  "isCompleted": true
}
```

**Response:** `200 OK`
```json
{
  "id": "c5dcd16f-e9fc-6449-bg6f-4394e8445386",
  "title": "Learn .NET 8",
  "description": "Study new features in .NET 8",
  "isCompleted": true,
  "createdAt": "2023-12-01T11:00:00Z",
  "completedAt": "2023-12-01T12:30:00Z",
  "userId": "a3bab04f-c7da-4227-9a4f-2192c6223164"
}
```

**Error Responses:**
- `404 Not Found` - Todo not found

#### Delete Todo
Deletes a todo item permanently.

**Endpoint:** `DELETE /todos/{id}`

**Path Parameters:**
- `id` (uuid) - Todo ID

**Response:** `204 No Content`

**Error Responses:**
- `404 Not Found` - Todo not found

## Data Models

### User
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| id | uuid | Yes | Unique identifier |
| name | string | Yes | User's full name (max 100 chars) |
| email | string | Yes | User's email address (unique) |
| createdAt | datetime | Yes | When user was created |

### TodoItem
| Field | Type | Required | Description |
|-------|------|----------|-------------|
| id | uuid | Yes | Unique identifier |
| title | string | Yes | Todo title (max 200 chars) |
| description | string | No | Todo description (max 1000 chars) |
| isCompleted | boolean | Yes | Completion status |
| createdAt | datetime | Yes | When todo was created |
| completedAt | datetime | No | When todo was completed |
| userId | uuid | Yes | ID of the user who owns this todo |

## Example Usage

### 1. Create a User
```bash
curl -X POST http://localhost:5119/api/users \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Alice Johnson",
    "email": "alice@example.com"
  }'
```

### 2. Create a Todo
```bash
curl -X POST http://localhost:5119/api/todos \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Review pull requests",
    "description": "Check team PRs before EOD",
    "userId": "a3bab04f-c7da-4227-9a4f-2192c6223164"
  }'
```

### 3. Get User's Todos
```bash
curl http://localhost:5119/api/users/a3bab04f-c7da-4227-9a4f-2192c6223164/todos
```

### 4. Mark Todo as Complete
```bash
curl -X PATCH http://localhost:5119/api/todos/c5dcd16f-e9fc-6449-bg6f-4394e8445386 \
  -H "Content-Type: application/json" \
  -d '{"isCompleted": true}'
```

### 5. Delete a Todo
```bash
curl -X DELETE http://localhost:5119/api/todos/c5dcd16f-e9fc-6449-bg6f-4394e8445386
```

## Error Handling

All error responses follow a consistent format:

```json
{
  "message": "Error description"
}
```

### Common HTTP Status Codes
- `200 OK` - Request successful
- `201 Created` - Resource created successfully
- `204 No Content` - Resource deleted successfully
- `400 Bad Request` - Invalid request data
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource conflict (e.g., duplicate email)

## Rate Limiting
Currently, no rate limiting is implemented. Consider implementing rate limiting for production use.

## Swagger Documentation
When running in development mode, interactive API documentation is available at:
```
http://localhost:5119/swagger
```

## Testing
The API includes comprehensive test coverage:
- Unit tests for business logic
- Integration tests for all endpoints
- In-memory database for test isolation

## Versioning
Current API version: v1

Future versions should be accessed via:
```
http://localhost:5119/api/v2/...
```