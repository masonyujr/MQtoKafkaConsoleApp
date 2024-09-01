# Use the official Microsoft .NET runtime image as a base image
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

# Use the official Microsoft .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project file and restore any dependencies
COPY ["MqToKafkaApp/MqToKafkaApp.csproj", "MqToKafkaApp/"]
RUN dotnet restore "MqToKafkaApp/MqToKafkaApp.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet build "MqToKafkaApp/MqToKafkaApp.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "MqToKafkaApp/MqToKafkaApp.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app

# Copy the built application
COPY --from=build /app/publish .

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "MqToKafkaApp.dll"]
# Use the official Microsoft .NET runtime image as a base image
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

# Use the official Microsoft .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project file and restore any dependencies
COPY ["MqToKafkaApp/MqToKafkaApp.csproj", "MqToKafkaApp/"]
RUN dotnet restore "MqToKafkaApp/MqToKafkaApp.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet build "MqToKafkaApp/MqToKafkaApp.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "MqToKafkaApp/MqToKafkaApp.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app

# Copy the built application
COPY --from=build /app/publish .

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "MqToKafkaApp.dll"]
