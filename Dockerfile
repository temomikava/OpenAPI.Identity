# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["OpenAPI.Identity/OpenAPI.Identity/OpenAPI.Identity.csproj", "OpenAPI.Identity/OpenAPI.Identity/"]
COPY ["OpenAPI.Identity/Contracts/IntegrationEvents.csproj", "Contracts/"]
COPY ["OpenAPI.Identity/SharedKernel/SharedKernel.csproj", "SharedKernel/"]
RUN dotnet restore "OpenAPI.Identity/OpenAPI.Identity/OpenAPI.Identity.csproj"

# Copy the entire project and build it
COPY . .
WORKDIR "/src/OpenAPI.Identity/OpenAPI.Identity"
RUN dotnet build "OpenAPI.Identity.csproj" -c Release -o /app/build

# Publish the application to a folder
FROM build AS publish
RUN dotnet publish "OpenAPI.Identity.csproj" -c Release -o /app/publish

# Copy the published application to the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenAPI.Identity.dll"]





