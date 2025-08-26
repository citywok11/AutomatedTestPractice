# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY src/TodoApi.Core/TodoApi.Core.csproj src/TodoApi.Core/
COPY src/TodoApi.Infrastructure/TodoApi.Infrastructure.csproj src/TodoApi.Infrastructure/
COPY src/TodoApi.API/TodoApi.API.csproj src/TodoApi.API/

# Restore dependencies for API project (which will restore all dependencies)
RUN dotnet restore src/TodoApi.API/TodoApi.API.csproj

# Copy source code
COPY src/ src/

# Build and publish
RUN dotnet publish src/TodoApi.API/TodoApi.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "TodoApi.API.dll"]