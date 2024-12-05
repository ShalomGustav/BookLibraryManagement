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
    - [Общие принципы Commands](#общие-принципы-commands)
  - [Controllers](#controllers)
    - [AuthorsController](#authorscontroller)
    - [BooksController](#bookscontroller)
    - [Общие принципы Controllers](#общие-принципы-controllers)
  - [Interfaces](#interfaces)
  - [Middlewares](#middlewares)
    - [Описание компонентов Middlewares](#описание-компонентов)
  - [Migrations](#migrations)
  - [Models](#models)
    - [Основные компоненты ](#основные-компоненты)
  - [Queries](#queries)
    - [Описание компонентов Queries](#описание-компонентов-queries)
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

# **Папка Commands** 

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

## Общие принципы Commands

1. **Разделение ответственности**:
   - Команда отвечает за описание действия, но не за его реализацию.
   - Логика выполнения команды сосредоточена в обработчике.

2. **Использование MediatR**:
   - Команды отправляются через `IMediator.Send()`.
   - Это упрощает взаимодействие между слоями и уменьшает связанность компонентов.

3. **Асинхронность**:
   - Все команды и обработчики реализованы асинхронно для повышения производительности.

---

# **Controllers**

**Контроллеры** отвечают за обработку HTTP-запросов, поступающих в API, и направляют их в соответствующие сервисы, команды или запросы. Контроллеры являются мостом между клиентом и бизнес-логикой приложения.

---

## Структура папки Controllers

- **AuthorsController.cs**  
- **BooksController.cs**  

---

## **Описание контроллеров**

### **AuthorsController**

1. **Описание**:
   - Контроллер для работы с авторами.
   - Использует MediatR для выполнения запросов.

2. **Маршруты**:
   - `[Route("api/authors")]` — базовый маршрут для работы с авторами.

3. **Методы**:
   - `[HttpGet] GetAllAuthorsAsync()`:
     - Возвращает список всех авторов.
     - Использует запрос `GetAllAuthorsQuery` через MediatR.
---

### **BooksController**

1. **Описание**:
   - Контроллер для работы с книгами.
   - Подключает `BookServices` для бизнес-логики и MediatR для выполнения команд и запросов.

2. **Маршруты**:
   - `[Route("api/books")]` — базовый маршрут для работы с книгами.

3. **Методы**:
   
   - `[HttpGet("test-error")] TestError()`:
     - Искусственно выбрасывает исключение для проверки обработки ошибок через middleware.

   - `[HttpGet] GetAllBooksAsync()`:
     - Возвращает список всех книг.
     - Использует запрос `GetAllBooksQuery` через `MediatR`.
       
   - `[HttpGet("{id}")] GetBookByIdAsync(Guid id)`:
     - Возвращает книгу по её уникальному идентификатору.
     - Использует запрос `GetBookByIdQuery` через `MediatR`.

   - `[HttpPost] CreateBookAsync(BookModelDto bookModel)`:
     - Добавляет новую книгу.
     - Использует команду `CreateBookCommand` через `MediatR`.

   - `[HttpPut("{id}")] UpdateBookAsync(Guid id, ...)`:
     - Обновляет информацию о книге.
     - Использует команду `UpdateBookCommand` через `MediatR`.

   - `[HttpDelete("{id}")] DeleteBookAsync(Guid id)`:
     - Удаляет книгу по её ID.
     - Использует команду `DeleteBookCommand` через `MediatR`.

---

# **Общие принципы Controllers**

1. **Использование MediatR**:
   - Уменьшает связанность между контроллерами и слоями бизнес-логики.

2. **Обработка ошибок**:
   - Исключения обрабатываются через middleware, что позволяет сократить количество проверок в контроллерах.

3. **Валидация**:
   - Валидация данных входного запроса осуществляется через `ModelState` и `FluentValidation`.

---

## **Interfaces**

Папка **Interfaces** предназначена для хранения интерфейсов. В текущей реализации проекта интерфейсы не используются напрямую, их наличие предусмотрено для будущих возможных расширений. 

---

## **Middlewares**

Папка **Middlewares** содержит пользовательские компоненты промежуточного ПО, которые обрабатывают HTTP-запросы и ответы. В данном проекте промежуточное ПО используется для демонстрации обработки ошибок.

---

## ErrorHandlingMiddleware

### Описание

`ErrorHandlingMiddleware` демонстрирует, как можно централизованно обрабатывать исключения в приложении. Этот компонент перехватывает любые необработанные исключения, возникающие в процессе обработки запросов, и возвращает стандартизированный JSON-ответ.

### Ключевые моменты

- **Перехват исключений**: Все исключения обрабатываются внутри метода `HandleExceptionAsync`.
- **JSON-ответ**: Клиент получает понятное сообщение об ошибке с подробной информацией.
- **Демонстрация**: Middleware включён в проект в качестве примера, его использование для данного проекта не является обязательным.

### Принципы работы

- Обёртка для запросов: Метод `Invoke` перехватывает запросы и обрабатывает исключения. 

- Формат ответа: Возвращается JSON-объект с ключами `Message`, `Detailed` и `ExceptionType` для удобного отображения ошибок.

- Регистрация: Middleware регистрируется в Program.cs и выполняется на начальном этапе обработки запросов.

- Использование: Middleware автоматически активируется при регистрации и централизует обработку всех исключений в приложении.

## **Migrations**

Папка `Migrations` создаётся автоматически при использовании инструмента миграций `Entity Framework Core` и содержит файлы, описывающие изменения в структуре базы данных. Эти файлы позволяют отслеживать, сохранять и применять изменения в схеме базы данных, упрощая управление её версиями и обеспечивая консистентность данных между различными средами разработки, тестирования и эксплуатации.

## **Models** 

Папка **Models** содержит классы, представляющие данные, используемые в приложении. Эти классы служат основой для работы с `Entity Framework Core`, а также используются для передачи данных между различными компонентами приложения.

---

## Основные компоненты

### **BookModel**

1. **Описание**:
   - Представляет сущность книги.
   - Используется для хранения информации о книгах в базе данных.

2. **Свойства**:
   - `Id` (Guid): Уникальный идентификатор книги.
   - `Title` (string): Название книги.
   - `Author` (BookAuthorModel): Ссылка на объект автора книги.
   - `PublishedYear` (int): Год издания книги.
   - `Genre` (string): Жанр книги.
   - `AuthorId` (Guid): Уникальный идентификатор автора.

### **BookAuthorModel**

1. **Описание**:
   - Представляет сущность автора книги.
   - Используется для хранения информации об авторах в базе данных.

2. **Свойства**:
   - `Id` (Guid): Уникальный идентификатор автора.
   - `FullName` (string): Полное имя автора.
   - `Birthday` (DateTime): Дата рождения автора.
   - `Book` (BookModel): Связанная книга (указана как `JsonIgnore` для предотвращения циклической сериализации).

### **BookModelDto**

1. **Описание**:
   - DTO (Data Transfer Object) для передачи данных книги между слоями приложения.

2. **Свойства**:
   - `Title` (string): Название книги.
   - `Author` (BookAuthorModelDto): DTO объекта автора.
   - `PublishedYear` (int): Год публикации книги.
   - `Genre` (string): Жанр книги.

3. **Методы**:
   - `CreateBook(BookModelDto book)`:
     - Создаёт объект `BookModel` из DTO.

### **BookAuthorModelDto**

1. **Описание**:
   - DTO для передачи данных автора между слоями приложения.

2. **Свойства**:
   - `FullName` (string): Полное имя автора.
   - `Birthday` (DateTime): Дата рождения.

3. **Методы**:
   - `CreateAuthor(BookAuthorModelDto bookAuthor)`:
     - Создаёт объект `BookAuthorModel` из DTO.

---

## Роль моделей

1. **Entity Framework Core**:
   - Модели используются для генерации таблиц базы данных.
   - Связи между моделями описывают связи между таблицами.

2. **Передача данных**:
   - DTO обеспечивают безопасный и упрощённый способ передачи данных между компонентами приложения.

---

## **Queries**

Папка `Queries` содержит запросы и их обработчики, реализующие логику извлечения данных из базы данных с использованием паттерна `CQRS`. Эти классы предназначены только для получения данных без их изменения.

---

## **Описание компонентов Queries**

### **GetAllAuthorsQuery**
- **Описание**:  
  Класс-запрос для получения списка всех авторов.
- **Результат**:  
  Возвращает список объектов `BookAuthorModel`, содержащих данные об авторах, такие как имя и дата рождения.
- **Обработчик**:  
  - Класс: `GetAllAuthorsQueryHandler`.
  - Логика: Использует метод `GetAllAuthorAsync` из `BookServices` для извлечения данных об авторах из базы данных.


### **GetAllBooksQuery**
- **Описание**:  
  Класс-запрос для получения списка всех книг.
- **Результат**:  
  Возвращает список объектов `BookModel`, содержащих информацию о книгах, включая заголовок, жанр, год издания и автора.
- **Обработчик**:  
  - Класс: `GetAllBooksQueryHandler`.
  - Логика: Использует метод `GetAllAsync` из `BookServices` для извлечения данных о книгах.


### **GetBookByIdQuery**
- **Описание**:  
  Класс-запрос для получения конкретной книги по её уникальному идентификатору (`Id`).
- **Результат**:  
  Возвращает объект `BookModel`, содержащий данные о книге, такой как заголовок, жанр, год издания и автор.
- **Обработчик**:  
  - Класс: `GetBookByIdQueryHandler`.
  - Логика: Использует метод `GetBookByIdAsync` из `BookServices` для получения книги по её `Id`.


### **GetAllAuthorsQueryHandler**
- **Описание**:  
  Обработчик для `GetAllAuthorsQuery`.
- **Роль**:  
  Выполняет логику извлечения данных об авторах, переданных в запросе `GetAllAuthorsQuery`.
- **Метод**:  
  - `Handle`: Использует метод `GetAllAuthorAsync` из `BookServices`, чтобы асинхронно вернуть список авторов.


### **GetAllBooksQueryHandler**
- **Описание**:  
  Обработчик для `GetAllBooksQuery`.
- **Роль**:  
  Выполняет логику извлечения списка всех книг.
- **Метод**:  
  - `Handle`: Асинхронно вызывает метод `GetAllAsync` из `BookServices` для получения всех записей о книгах.


### **GetBookByIdQueryHandler**
- **Описание**:  
  Обработчик для `GetBookByIdQuery`.
- **Роль**:  
  Извлекает данные о книге по её уникальному идентификатору.
- **Метод**:  
  - `Handle`: Асинхронно вызывает метод `GetBookByIdAsync` из `BookServices` для извлечения данных книги.

---







