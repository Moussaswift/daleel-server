# Use .NET SDK for build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy .csproj and restore as distinct layers
COPY daleel.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use runtime-only image for final output
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set the entrypoint to run the app
ENTRYPOINT ["dotnet", "daleel.dll"]