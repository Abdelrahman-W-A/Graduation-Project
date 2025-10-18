# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln ./
COPY Demo.PL/*.csproj Demo.PL/
COPY Demo.BLL/*.csproj Demo.BLL/
COPY Demo.DAL/*.csproj Demo.DAL/
RUN dotnet restore Demo.PL/Demo.PL.csproj

COPY . .
RUN dotnet publish Demo.PL/Demo.PL.csproj -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 5000
ENTRYPOINT ["dotnet", "Demo.PL.dll"]