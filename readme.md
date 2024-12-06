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
    - [Описание компонентов Middlewares](#описание-компонентов-middlewares)
  - [Migrations](#migrations)
  - [Models](#models)
    - [Основные компоненты ](#основные-компоненты)
  - [Queries](#queries)
    - [Описание компонентов Queries](#описание-компонентов-queries)
  - [Repositories](#repositories)
    - [Основные элементы класса BookDbContext](#основные-элементы-класса-bookdbcontext)
    - [Методы класса BookDbContext](#методы-класса-bookdbcontext)
  - [Services](#services)
    - [Взаимодействие с MediatR](#взаимодействие-с-mediatr)
    - [Основные методы](#основные-методы)
    - [Особенности реализации](#особенности-реализации)
  - [Validators](#validators)
- [Конфигурация проекта. Файл appsettings.json](#конфигурация-проекта.-файл-appsettings.json)
  - [Основные настройки](#основные-астройки)
- [Program.cs](#program.cs)
  - [Основные компоненты Program.cs](#основные-компоненты-program.cs)
- [Тестирование приложения](#тестирование-приложения)
  


## **Требования**

- .NET 6.0 SDK или выше
- Microsoft SQL Server
- Среда разработки: Visual Studio 2022 / Rider / VS Code
  
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

## **Описание компонентов Middlewares**

### ErrorHandlingMiddleware


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

## **Repositories**

Класс `BookDbContext`  в папке Repositories — это основа взаимодействия приложения с базой данных, реализованная с использованием Entity Framework Core. Он предоставляет доступ к таблицам базы данных через объекты `DbSet<T>`, а также управляет их конфигурацией и связями.

---

## **Основные элементы класса BookDbContext**

### **DbSet<T>**

1. **Books**:
   - Представляет таблицу `Books` в базе данных.
   - Используется для выполнения операций CRUD (создание, чтение, обновление, удаление) над объектами типа `BookModel`.
   - Содержит данные о книгах, такие как:
     - `Title` — название книги.
     - `PublishedYear` — год публикации.
     - `Genre` — жанр книги.
     - `AuthorId` — идентификатор автора.

2. **BookAuthor**:
   - Представляет таблицу `BookAuthor` в базе данных.
   - Используется для работы с объектами типа `BookAuthorModel`.
   - Содержит данные об авторах, такие как:
     - `FullName` — полное имя автора.
     - `Birthday` — дата рождения автора.

---

### Конструкторы

1. **BookDbContext()**:
   - Конструктор по умолчанию.
   - Может быть использован для тестирования без необходимости передачи параметров конфигурации.

2. **BookDbContext(DbContextOptions<BookDbContext> options)**:
   - Конструктор, принимающий объект `DbContextOptions<BookDbContext>`, содержащий параметры конфигурации для подключения к базе данных.
   - Этот конструктор используется при внедрении зависимостей (Dependency Injection) для настройки подключения к базе данных в `Program.cs`.

---

### **Методы класса BookDbContext**

#### **OnModelCreating(ModelBuilder modelBuilder)**

Этот метод используется для конфигурации моделей и настройки их отображения на таблицы базы данных. Он вызывается при инициализации контекста базы данных.

1. **Конфигурация таблицы `Books`**:
   - Настраивается как таблица с именем `Books`.
   - Поля:
     - `Id`:
       - Является первичным ключом.
       - Автоматически генерируется.
     - `Title`:
       - Хранит название книги.
       - Максимальная длина: 256 символов.
     - `PublishedYear`:
       - Год публикации книги.
     - `Genre`:
       - Жанр книги.
       - Максимальная длина: 128 символов.

2. **Конфигурация таблицы `BookAuthor`**:
   - Настраивается связь между `BookAuthorModel` и `BookModel` как "один-к-одному".
   - Навигационные свойства:
     - Класс `BookAuthorModel` имеет ссылку на объект `BookModel`.
     - Класс `BookModel` имеет ссылку на объект `BookAuthorModel` через свойство `Author`.
   - Внешний ключ:
     - Поле `AuthorId` в таблице `Books` ссылается на запись в таблице `BookAuthor`.
   - Удаление:
     - Связанные записи удаляются каскадно (`OnDelete(DeleteBehavior.Cascade)`).

Роль `BookDbContext` в проекте:

- Определение схемы базы данных: Класс управляет отображением моделей на таблицы базы данных, включая их поля и связи.
  
- Управление миграциями: Изменения в этом классе позволяют автоматически генерировать и применять миграции базы данных.

- Работа с данными: Обеспечивает интерфейс для выполнения запросов к базе данных, абстрагируя сложности низкоуровневых операций `SQL`.

---

# **BookServices**

`BookServices` — это класс, реализующий бизнес-логику для управления книгами и авторами. Этот класс взаимодействует с контекстом базы данных (`BookDbContext`) для выполнения операций CRUD (создание, чтение, обновление, удаление) с сущностями `BookModel` и `BookAuthorModel`.

---

## **Взаимодействие с MediatR**

Класс `BookServices` используется в связке с MediatR для выполнения бизнес-логики через команды и запросы.

### Основные взаимодействия

1. **Команды (Commands)**:
   - Методы, такие как `CreateBookAsync`, `UpdateBookAsync` и `DeleteBookAsync`, вызываются через обработчики MediatR для выполнения операций изменения данных.

2. **Запросы (Queries)**:
   - Методы `GetAllAsync`, `GetAllAuthorAsync` и `GetBookByIdAsync` вызываются через обработчики MediatR для извлечения данных.

**Роль MediatR**:
- Уменьшение зависимости между контроллерами и бизнес-логикой.
- Инкапсуляция логики выполнения команд и запросов в обработчиках.
- Упрощение масштабирования приложения.

---

## **Services**

## **Основные методы**

### **GetAllAsync(CancellationToken ctx)**
- **Описание**:  
  Возвращает список всех книг с их авторами.
- **Логика**:  
  Использует `Include` для загрузки данных об авторах (`Author`) и выполняет запрос к таблице `Books`.
- **Результат**:  
  Список объектов `BookModel`.
- **Взаимодействие**:  
  Используется в `GetAllBooksQueryHandler`, который обрабатывает запрос `GetAllBooksQuery`.

---

### **GetAllAuthorAsync(CancellationToken ctx)**
- **Описание**:  
  Возвращает список всех авторов.
- **Логика**:  
  Выполняет запрос к таблице `BookAuthor` для получения всех записей.
- **Результат**:  
  Список объектов `BookAuthorModel`.
- **Взаимодействие**:  
  Используется в `GetAllAuthorsQueryHandler`, который обрабатывает запрос `GetAllAuthorsQuery`.

---

### **GetBookByIdAsync(Guid id, CancellationToken ctx)**
- **Описание**:  
  Возвращает книгу по её уникальному идентификатору (ID).
- **Логика**:  
  Проверяет, что ID не пустой (`Guid.Empty`), и выполняет запрос с использованием `Include` для загрузки данных об авторе книги.
- **Результат**:  
  Объект `BookModel` с подробной информацией о книге.
- **Взаимодействие**:  
  Используется в `GetBookByIdQueryHandler`, который обрабатывает запрос `GetBookByIdQuery`.

---

### **CreateBookAsync(BookModel bookModel, CancellationToken ctx)**
- **Описание**:  
  Создаёт новую книгу.
- **Логика**:  
  Добавляет новую запись в таблицу `Books` и сохраняет изменения.
- **Результат**:  
  Возвращает объект созданной книги.
- **Взаимодействие**:  
  Используется в `CreateBookCommandHandler`, который обрабатывает команду `CreateBookCommand`.

---

### **UpdateBookAsync(Guid id, string title, string genre, int publishedYear, CancellationToken ctx)**
- **Описание**:  
  Обновляет данные существующей книги.
- **Логика**:  
  Находит книгу по её ID, обновляет её поля (`Title`, `Genre`, `PublishedYear`) и сохраняет изменения.
- **Результат**:  
  Изменения сохраняются в базе данных.
- **Взаимодействие**:  
  Используется в `UpdateBookCommandHandler`, который обрабатывает команду `UpdateBookCommand`.

---

### **DeleteBookAsync(Guid id, CancellationToken ctx)**
- **Описание**:  
  Удаляет книгу по её уникальному идентификатору.
- **Логика**:  
  Ищет книгу по её ID, удаляет запись из базы данных, если книга найдена, и сохраняет изменения.
- **Результат**:  
  Возвращает `true`, если книга успешно удалена, или `false`, если книга не найдена.
- **Взаимодействие**:  
  Используется в `DeleteBookCommandHandler`, который обрабатывает команду `DeleteBookCommand`.

---

## **Особенности реализации**

1. **Асинхронные операции**:  
   Все методы используют `async/await`, что позволяет работать с базой данных без блокировки основного потока.

2. **Взаимодействие с `BookDbContext`**:  
   Методы используют контекст базы данных для выполнения операций над таблицами `Books` и `BookAuthor`.

3. **Обработка данных**:  
   Класс проверяет корректность входных данных (например, наличие ID) перед выполнением операций.

4. **Загрузка связанных данных**:  
   Методы, такие как `GetAllAsync` и `GetBookByIdAsync`, используют `Include` для подгрузки связанных данных.

# Validators

## Назначение

Папка `Validators` содержит классы, реализующие валидацию входных данных для команд в проекте. Это обеспечивает проверку данных на уровне бизнес-логики перед выполнением команд, снижая вероятность ошибок и улучшая консистентность данных.

---

## Валидаторы

### **CreateBookCommandValidator**

- **Назначение**:  
  Выполняет валидацию данных команды `CreateBookCommand`, которая создаёт новую книгу.

- **Правила валидации**:
  1. Поле `Title` книги:
     - Должно быть заполнено (`NotEmpty`) и не быть `null`.
  2. Поле `PublishedYear`:
     - Должно содержать только цифры. Используется метод `IsDigitsOnly`.
     - Выдаёт сообщение об ошибке: `"State number only"`.
  3. Поле `Genre`:
     - Должно быть заполнено (`NotEmpty`) и не быть `null`.
  4. Поле `FullName` автора:
     - Должно быть заполнено (`NotEmpty`) и соответствовать регулярному выражению `^[a-zA-Z0-9\s]{1,30}$`.
     - Выдаёт сообщение об ошибке: `"Full name can only contain letters, numbers, and spaces, max 30 chars."`.
  5. Поле `Birthday` автора:
     - Должно быть заполнено (`NotEmpty`).
     - Не должно быть равно `DateTime.MinValue` или `DateTime.Today`.
     - Выдаёт сообщение об ошибке: `"Birthday must be a valid past date."`.

---

### **DeleteBookCommandValidator**

- **Назначение**:  
  Выполняет валидацию данных команды `DeleteBookCommand`, которая удаляет книгу.

- **Правила валидации**:
  1. Поле `ID`:
     - Должно быть заполнено (`NotEmpty`) и не быть `null`.

---

### **UpdateBookCommandValidator**

- **Назначение**:  
  Выполняет валидацию данных команды `UpdateBookCommand`, которая обновляет существующую книгу.

- **Правила валидации**:
  1. Поле `ID`:
     - Должно быть заполнено (`NotEmpty`) и не быть `null`.
  2. Поле `PublishedYear`:
     - Должно быть в диапазоне от 1 до 9999 (`InclusiveBetween`).
     - Выдаёт сообщение об ошибке: `"Range on 1 to 9999"`.
  3. Поле `Title`:
     - Должно быть заполнено (`NotEmpty`) и не быть `null`.
  4. Поле `Genre`:
     - Должно быть заполнено (`NotEmpty`) и соответствовать регулярному выражению `^[a-zA-Z0-9\s]{1,30}$`.
     - Выдаёт сообщение об ошибке: `"Full name can only contain letters, numbers, and spaces, max 30 chars."`.

---

## Общие принципы

1. **Использование FluentValidation**:  
   Валидаторы наследуются от `AbstractValidator<T>` и используют декларативные правила для проверки входных данных.

2. **Централизованная обработка ошибок**:  
   Ошибки валидации автоматически перехватываются и возвращаются клиенту, упрощая обработку исключений в коде.

3. **Логика без использования регулярных выражений**:  
   Для проверки числовых данных (`PublishedYear`) используется метод `IsDigitsOnly`, который проверяет строку символ за символом.

---

## **Конфигурация проекта. Файл appsettings.json**

### **Файл appsettings.json**

Файл `appsettings.json` содержит ключевые настройки для приложения, включая логирование, строки подключения к базе данных и разрешённые хосты.

### Пример файла:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(local);Initial Catalog=BookLibrary;Persist Security Info=True;User ID=test;Password=test;MultipleActiveResultSets=True;Connect Timeout=30;TrustServerCertificate=True"
  },
  "AllowedHosts": "*"
}
```
## **Основные настройки**

`ConnectionStrings`:

- `DefaultConnection`:
  
  - Строка подключения к базе данных `MS SQL Server`. Она включает:

  - `Data Source`: Сервер базы данных.

  - `Initial Catalog`: Имя базы данных (в проекте, `BookLibrary`).

- `User` `ID` и `Password`: Учетные данные для подключения.

- TrustServerCertificate: Указывает на доверие к сертификатам сервера.

- AllowedHosts: Указывает, какие хосты разрешены. "*" означает, что приложение принимает запросы от всех хостов.
  
---

## **Program.cs**

Файл `Program.cs` представляет точку входа в приложение ASP.NET Core, определяет настройки зависимостей, middleware и маршрутов. Он управляет взаимодействием компонентов приложения, включая базы данных, авторизацию и обработку HTTP-запросов.

### Основные компоненты

#### Настройка конфигурации приложения:
```csharp
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

```
- Используется для загрузки конфигураций из файла `appsettings.json`.
  
#### Регистрация сервисов:
  
```csharp
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
```
- Регистрируется контекст базы данных `BookDbContext`, использующий `SQL` Server с настройками из `appsettings.json`.
  
---

```csharp
builder.Services.AddScoped<BookServices>();
```
- Регистрируется сервис `BookService` с областью действия `Scoped`.

---

```csharp
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));
```
- Используется библиотека `MediatR` для реализации паттерна `CQRS`.

---

```csharp
builder.Services.AddFluentValidationAutoValidation();
```

- Автоматическая регистрация валидаторов `FluentValidation`.

---

```csharp
builder.Services.AddAuthorization();
builder.Services.AddControllers();
```
- Добавляется поддержка авторизации и регистрация контроллеров.
  
---

#### Настройка Swagger:

```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```

- `Swagger` используется для документирования `API`.

  ---

 #### Конвейер обработки запросов:

```csharp
 app.UseMiddleware<ErrorHandlingMiddleware>();
```

- Добавляется промежуточное ПО(`Middleware`) для обработки ошибок.

---

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

- `Swagger` доступен только в режиме разработки.

---

```csharp
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
```

- Настраиваются редирект на `HTTPS`, авторизация и маршрутизация к контроллерам.

#### Запуск приложения:

```scharp
app.Run();
```

- Запускается веб-приложение.

---
