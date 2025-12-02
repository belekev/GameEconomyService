# --------------------------------------------------------------------------
# 1. BUILD STAGE (Используется для компиляции и публикации)
# --------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл решения (SLN) и все файлы проектов для восстановления зависимостей
# Это позволяет Docker использовать кеш слоев, если файлы проекта не менялись
COPY code/*.sln ./
COPY code/GameEconomyService.Application.Backend/*.csproj code/GameEconomyService.Application.Backend/
COPY code/GameEconomyService.Application.Core/*.csproj code/GameEconomyService.Application.Core/
COPY code/GameEconomyService.Contracts/*.csproj code/GameEconomyService.Contracts/
COPY code/GameEconomyService.Domain.Interfaces/*.csproj code/GameEconomyService.Domain.Interfaces/
COPY code/GameEconomyService.Infrastructure.ServiceTools/*.csproj code/GameEconomyService.Infrastructure.ServiceTools/

# Восстановление зависимостей
RUN dotnet restore code/GameEconomyService.Application.Backend/GameEconomyService.Application.Backend.csproj

# Копируем весь исходный код
COPY code/ code/

# Публикация основного проекта бэкенда
# Заменяем GameEconomyService.Application.Backend.csproj на его путь относительно WORKDIR
RUN dotnet publish code/GameEconomyService.Application.Backend/GameEconomyService.Application.Backend.csproj -c Release -o /app/publish --no-restore

# --------------------------------------------------------------------------
# 2. FINAL STAGE (Используется для запуска)
# --------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем опубликованные файлы из предыдущего этапа
COPY --from=build /app/publish .

# Открываем порты для HTTP (8080) и SignalR (должен использовать тот же порт)
EXPOSE 8080 

# Точка входа: запускаем собранную DLL
# Обратите внимание: имя DLL должно совпадать с именем проекта
ENTRYPOINT ["dotnet", "GameEconomyService.Application.Backend.dll"]
