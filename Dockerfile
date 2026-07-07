# Etapa 1 — compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["NaturalezaViva.Api/NaturalezaViva.Api.csproj", "NaturalezaViva.Api/"]
COPY ["NaturalezaViva.Application/NaturalezaViva.Application.csproj", "NaturalezaViva.Application/"]
COPY ["NaturalezaViva.Domain/NaturalezaViva.Domain.csproj", "NaturalezaViva.Domain/"]
COPY ["NaturalezaViva.Infrastructure/NaturalezaViva.Infrastructure.csproj", "NaturalezaViva.Infrastructure/"]

RUN dotnet restore "NaturalezaViva.Api/NaturalezaViva.Api.csproj"

COPY . .

RUN dotnet publish "NaturalezaViva.Api/NaturalezaViva.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Etapa 2 — ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "NaturalezaViva.Api.dll"]