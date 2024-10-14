# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln ./
COPY TenexCars/TenexCars.csproj TenexCars/
COPY TenexCars.DataAccess/TenexCars.DataAccess.csproj TenexCars.DataAccess/
RUN dotnet restore

# Copy the entire project and build
COPY . ./
WORKDIR /app/TenexCars
RUN dotnet publish -c Release -o out

# Stage 2: Set up the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/TenexCars/out .

# Expose the HTTP port
EXPOSE 80

# Set the entry point to the API project DLL
ENTRYPOINT ["dotnet", "TenexCars.dll"]