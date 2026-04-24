# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln ./
COPY src/Gumas.Domain/*.csproj ./src/Gumas.Domain/
COPY src/Gumas.Application/*.csproj ./src/Gumas.Application/
COPY src/Gumas.Infrastructure/*.csproj ./src/Gumas.Infrastructure/
COPY src/Gumas.Web/*.csproj ./src/Gumas.Web/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY src/ ./src/

# Build and publish
WORKDIR /src/src/Gumas.Web
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install cultures for Turkish support and curl for healthcheck
RUN apt-get update && apt-get install -y locales curl && \
    sed -i '/tr_TR.UTF-8/s/^# //g' /etc/locale.gen && \
    locale-gen && \
    rm -rf /var/lib/apt/lists/*

ENV LANG=tr_TR.UTF-8
ENV LANGUAGE=tr_TR:tr
ENV LC_ALL=tr_TR.UTF-8

# Copy published app
COPY --from=build /app/publish .

# Create directory for SQLite database (if used)
RUN mkdir -p /app/data

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "Gumas.Web.dll"]
