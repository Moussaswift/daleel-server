# Use .NET SDK for build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY daleel.csproj ./daleel.csproj
RUN dotnet restore daleel.csproj

# Copy the entire project directory and publish
COPY . ./
RUN dotnet publish daleel.csproj -c Release -o out

# Use runtime-only image for final output
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set the entrypoint to run the app
ENTRYPOINT ["dotnet", "daleel.dll"]