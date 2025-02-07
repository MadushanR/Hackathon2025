# Use the official .NET image from Microsoft for the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the HackathonProject.csproj file first
COPY ./Backend/HackathonProject/HackathonProject.csproj ./HackathonProject/

# Run dotnet restore to restore dependencies
RUN dotnet restore "./HackathonProject/HackathonProject.csproj"

# Copy the rest of the HackathonProject source files
COPY ./Backend/HackathonProject/ ./HackathonProject/

# Build the application
WORKDIR "/src/HackathonProject"
RUN dotnet build "HackathonProject.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "HackathonProject.csproj" -c Release -o /app/publish

# Final image with the published app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "HackathonProject.dll"]
