# Stage 1: Build với .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["AiTestSystem.csproj", "."]
RUN dotnet restore "AiTestSystem.csproj"

COPY . .
RUN dotnet publish "AiTestSystem.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime với .NET 10 preview
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Railway inject PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "AiTestSystem.dll"]
