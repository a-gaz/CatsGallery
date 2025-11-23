FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

## Копирование всех файлов проекта
COPY ["CatGallery2.Web/CatGallery2.Web.csproj", "CatGallery2.Web/"]
COPY ["CatGallery2.Application/CatGallery2.Application.csproj", "CatGallery2.Application/"]
COPY ["CatGallery2.Infrastructure.CatApi/CatGallery2.Infrastructure.CatApi.csproj", "CatGallery2.Infrastructure.CatApi/"]
COPY ["CatGallery2.Infrastructure.Minio/CatGallery2.Infrastructure.Minio.csproj", "CatGallery2.Infrastructure.Minio/"]
COPY ["CatGallery2.Infrastructure.PostgresStorage/CatGallery2.Infrastructure.PostgresStorage.csproj", "CatGallery2.Infrastructure.PostgresStorage/"]
COPY ["CatGallery2.Infrastructure.RedisStorage/CatGallery2.Infrastructure.RedisStorage.csproj", "CatGallery2.Infrastructure.RedisStorage/"]

RUN dotnet restore "CatGallery2.Web/CatGallery2.Web.csproj"

COPY . .
RUN dotnet build "CatGallery2.Web/CatGallery2.Web.csproj" -c Release -o /app/build --no-restore

# Публикация
FROM build AS publish
RUN dotnet publish "CatGallery2.Web/CatGallery2.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Запуск
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatGallery2.Web.dll"]