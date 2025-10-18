# ============================
# Stage 1: Build
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY Demo.PL/Demo.PL.csproj Demo.PL/
RUN dotnet restore Demo.PL/Demo.PL.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish Demo.PL/Demo.PL.csproj -c Release -o /app/publish

# ============================
# Stage 2: Runtime
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Render sets a $PORT environment variable automatically
ENV ASPNETCORE_URLS=http://+:$PORT

# Expose that port for clarity (Render will still override)
EXPOSE 10000

# Run the application
ENTRYPOINT ["dotnet", "Demo.PL.dll"]
