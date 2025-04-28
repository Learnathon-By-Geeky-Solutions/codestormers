# Use an official .NET runtime as a parent image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Create a non-root user and group
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the backend project file and restore dependencies
COPY ["backend/CosmoVerse/CosmoVerse/CosmoVerse.csproj", "backend/CosmoVerse/CosmoVerse/"]
RUN dotnet restore "backend/CosmoVerse/CosmoVerse/CosmoVerse.csproj"

# Copy the rest of the backend code explicitly
COPY backend/CosmoVerse /src/backend/CosmoVerse/

# Build the application
WORKDIR "/src/backend/CosmoVerse/CosmoVerse"
RUN dotnet build "CosmoVerse.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "CosmoVerse.csproj" -c Release -o /app/publish

# Use the base runtime image for the final stage and copy the publish output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the default non-root user
USER appuser

# Set the entry point for the application
ENTRYPOINT ["dotnet", "CosmoVerse.dll"]
