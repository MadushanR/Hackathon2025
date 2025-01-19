# Use the official .NET image from Microsoft for the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY ./Backend/HackathonProject/HackathonProject.csproj ./HackathonProject/
RUN dotnet restore "HackathonProject/HackathonProject.csproj"

# Copy the entire project and publish
COPY ./Backend/HackathonProject/ ./HackathonProject/
RUN dotnet publish "HackathonProject/HackathonProject.csproj" -c Release -o out

# Final image with the published app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT ["dotnet", "HackathonProject.dll"]
