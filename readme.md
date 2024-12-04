# BookLibraryManagement

**Автор**: Фатихов Максим Сергеевич

## Описание

**Book Library Management** — это RESTful API для управления библиотекой книг, разработанное с использованием .NET. Проект предоставляет функционал для управления книгами и авторами с поддержкой CRUD-операций, использует базу данных MS SQL и реализует основные принципы архитектуры.

## Функционал

- **CQRS**: Разделение команд и запросов, что позволяет отделить операции изменения данных от их получения.
- **MediatR**: Используется для реализации CQRS и уменьшения зависимости между компонентами.
- **FluentValidation**: Автоматическая валидация данных на основе чётких правил.
- **Entity Framework Core**: ORM для работы с базой данных MS SQL Server.
- **Swagger**: Генерация документации и тестирование API через UI.
- **Middleware**: Пользовательские промежуточные слои для обработки исключений и логирования.

## Содержание 
- [Требования](#требования)
- [Структура проекта](#структура-проекта)
  - [Commands](#commands)
    - [Описание команд](#описание-команд)
  - [Controllers](#controllers)
  - [Interfaces](#interfaces)
  - [Middlewares](#middlewares)
  - [Migrations](#migrations)
  - [Models](#models)
  - [Queries](#queries)
  - [Repositories](#repositories)
  - [Services](#srevices)
  - [Validators](#validators)
- [Подключение к базе данных](#подключение-к-базе-данных)
  - [Строка подключения, appsetings](#строка-подключения-appsettings)
- [Валидация, middlewares, тестирование](#конфигурация-аутентификация-тестирование)
  - [Настройка Swagger](#настройка-swagger)
  - [Настройка аутентификации и авторизации](#настройка-аутентификации-и-авторизации)
  - [Применение миграций](#применение-миграций)


## **Требования**

- .NET 7
- SQL Server
  
Проект использует следующие библиотеки и пакеты:
- `Entity Framework Core`
- `MS SQL Server`
- `FluentValidation`
- `CQRS (Commands/Queries)` через `MediatR`
- `Swagger` для документации `API`

## **Структура проекта**

# **Commands** 

**Команды** реализуют часть паттерна CQRS и предназначены для изменения состояния данных приложения (создание, обновление или удаление). Каждая команда включает описание данных, которые требуется изменить, и обработчик, реализующий логику выполнения команды. В этом проекте команды реализованы с использованием библиотеки **MediatR**.

---
## Структура папки `Commands`

- `CreateBookCommand.cs`
- `CreateBookCommandHandler.cs`
- `DeleteBookCommand.cs`
- `DeleteBookCommandHandler.cs`
- `UpdateBookCommand.cs`
- `UpdateBookCommandHandler.cs`
---

## **Описание команд**

### **CreateBookCommand**

1. **Описание**:
   - Команда для добавления новой книги.
   - Передаёт объект `BookModel` с данными книги.
     
2. **Обработчик команды**:
   - Класс: `CreateBookCommandHandler`.
   - Обрабатывает запрос, вызывая метод `CreateBookAsync` из `BookServices`.
   - Возвращает созданную книгу.

### **UpdateBookCommand**

1. **Описание**:
   - Команда для обновления существующей книги.
   - Получает ID книги и новые значения полей (`Title`, `Genre`, `PublishedYear`).

2. **Обработчик команды**:
   - Класс: `UpdateBookCommandHandler`.
   - Вызывает метод `UpdateBookAsync` из `BookServices`.

---

### **DeleteBookCommand**

1. **Описание**:
   - Команда для удаления книги по её уникальному идентификатору (`ID`).

2. **Обработчик команды**:
   - Класс: `DeleteBookCommandHandler`.
   - Вызывает метод `DeleteBookAsync` из `BookServices`.
---

## Общие принципы

1. **Разделение ответственности**:
   - Команда отвечает за описание действия, но не за его реализацию.
   - Логика выполнения команды сосредоточена в обработчике.

2. **Использование MediatR**:
   - Команды отправляются через `IMediator.Send()`.
   - Это упрощает взаимодействие между слоями и уменьшает связанность компонентов.

3. **Асинхронность**:
   - Все команды и обработчики реализованы асинхронно для повышения производительности.

---

## Использование в проекте

1. **Пример отправки команды в контроллере**:
   ```csharp
   [HttpPost]
   public async Task<IActionResult> CreateBookAsync([FromBody] BookModelDto bookModel, CancellationToken ctx)
   {
       var model = BookModelDto.CreateBook(bookModel);
       var result = await _mediator.Send(new CreateBookCommand(model), ctx);
       return Ok(result);
   }
   ```
   - Здесь создаётся объект `CreateBookCommand` с данными книги и отправляется через `IMediator`.






