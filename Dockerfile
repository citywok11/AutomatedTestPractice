FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TodoApi.Api/TodoApi.Api.csproj", "src/TodoApi.Api/"]
COPY ["src/TodoApi.Infrastructure/TodoApi.Infrastructure.csproj", "src/TodoApi.Infrastructure/"]
COPY ["src/TodoApi.Core/TodoApi.Core.csproj", "src/TodoApi.Core/"]
RUN dotnet restore "src/TodoApi.Api/TodoApi.Api.csproj"
COPY . .
WORKDIR "/src/src/TodoApi.Api"
RUN dotnet build "TodoApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TodoApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.Api.dll"]