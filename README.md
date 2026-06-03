# WarehouseRentalApp – Веб-приложение для аренды склада

## Описание проекта
Веб-приложение для аренды складских помещений. Реализовано на ASP.NET Core (многослойная архитектура: Application, Domain, Infrastructure). База данных поднимается в Docker, само приложение также может быть запущено в Docker или напрямую через Visual Studio.

## Технологии
- ASP.NET Core (C#)
- Entity Framework Core (миграции)
- Docker / Docker Compose
- База данных: SQL Server (образ `mcr.microsoft.com/mssql/server`)
- Frontend: HTML, CSS
- Swagger / OpenAPI (документация API)

## Требования для запуска
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (обязательно, если используете Docker Compose)
- [.NET 6/7/8 SDK](https://dotnet.microsoft.com/en-us/download) (если запускаете без Docker)
- Visual Studio 2022 (рекомендуется)

## Быстрый запуск (рекомендуемый – через Docker Compose)

1. **Клонируйте репозиторий**  
```bash
   git clone https://github.com/ваш-логин/WarehouseRentalApp.git
   cd WarehouseRentalApp
```

2.Убедитесь, что Docker Desktop запущен.

3.Запустите сборку и поднятие контейнеров
 ```bash
docker-compose up -d
 ```

Эта команда поднимет:

-контейнер с базой данных (SQL Server)

-контейнер с веб-приложением (если docker-compose.yml настроен на сборку)

4.Примените миграции базы данных (если они не применяются автоматически).
Войдите в контейнер приложения или выполните:
```bash
docker exec -it <имя_контейнера_приложения> dotnet ef database update --project Infrastructure --startup Application
```
Либо откройте окно команд в Visual Studio и выберите «Применить миграции».

5.Откройте приложение в браузере

По умолчанию: http://localhost:8080 (или порт, указанный в docker-compose.yml).


## Документация API (Swagger)

После запуска документация API доступна по адресу:

При запуске через Visual Studio (Kestrel):
```bash
https://localhost:5001/swagger или http://localhost:5000/swagger (порт может отличаться)
```

При запуске через Docker Compose:
```bash
http://localhost:8080/swagger (или порт, указанный в docker-compose.yml для веб-приложения)
```

Swagger позволяет просмотреть все эндпоинты, их параметры и ответы, а также выполнить тестовые запросы.


## Примечания для преподавателя

Все настройки (пароли, строки подключения) находятся в appsettings.json и могут быть видны публично, так как это учебный проект с локальной базой данных.

Для проверки достаточно скачать репозиторий, запустить docker-compose up -d и открыть http://localhost:8080/swagger для тестирования API.

