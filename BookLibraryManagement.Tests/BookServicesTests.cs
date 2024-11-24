using BookLibraryManagement.Interfaces;
using BookLibraryManagement.Models;
using BookLibraryManagement.Repositories;
using BookLibraryManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryManagement.Tests
{
    public class BookServicesTests
    {
        [Fact]
        public async Task GetAllAsyncTests()
        {
            // Arrange: подготовка теста
            var mockBookService = new Mock<IBookServices>();
            // Создаем мок (поддельный объект), который реализует интерфейс IBookServices.
            // Это позволит настроить его методы для возврата тестовых данных.

            mockBookService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookModel>
                {
                // Настраиваем метод GetAllAsync так, чтобы он возвращал список из двух книг.
                new BookModel
                {
                    Id = Guid.NewGuid(), // Генерируем уникальный идентификатор
                    Title = "Book 1", // Задаем название книги
                    Author = new BookAuthorModel { FullName = "Author 1" } // Задаем автора книги
                },
                new BookModel
                {
                    Id = Guid.NewGuid(), // Генерируем уникальный идентификатор
                    Title = "Book 2", // Задаем название второй книги
                    Author = new BookAuthorModel { FullName = "Author 2" } // Задаем автора второй книги
                }
                });
            // Настройка мока завершена: теперь метод GetAllAsync всегда возвращает этот список.

            // Act: выполнение тестируемого метода
            var result = await mockBookService.Object.GetAllAsync(CancellationToken.None);
            // Вызываем метод GetAllAsync у настроенного мока.
            // CancellationToken.None указывает, что отмена вызова не предполагается.

            // Assert: проверка результатов
            Assert.NotNull(result);
            // Проверяем, что результат не является null.

            Assert.Equal(2, result.Count);
            // Проверяем, что список содержит ровно две книги.

            Assert.Equal("Book 1", result[0].Title);
            // Проверяем, что название первой книги в списке соответствует "Book 1".

            Assert.Equal("Author 1", result[0].Author.FullName);
            // Проверяем, что имя автора первой книги соответствует "Author 1".

            Assert.Equal("Book 2", result[1].Title);
            // Проверяем, что название второй книги в списке соответствует "Book 2".

            Assert.Equal("Author 2", result[1].Author.FullName);
            // Проверяем, что имя автора второй книги соответствует "Author 2".
        }
    }
}
