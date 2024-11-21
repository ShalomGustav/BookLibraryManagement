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
        //[Fact]
        //public async Task GetAllAsyncTests()
        //{
        //    // Создаем изолированный ServiceProvider для тестов
        //    var serviceProvider = ConfigureInMemoryServices();

        //    using var scope = serviceProvider.CreateScope();
        //    var context = scope.ServiceProvider.GetRequiredService<BookDbContext>();

        //    // Добавляем тестовые данные
        //    context.Books.Add(new BookModel { Id = Guid.NewGuid(), Title = "Test Book", Genre = "Fiction", PublishedYear = 2022 });
        //    context.SaveChanges();

        //    // Используем BookServices из ServiceProvider
        //    var service = scope.ServiceProvider.GetRequiredService<BookServices>();

        //    // Вызываем метод сервиса
        //    var result = await service.GetAllAsync(CancellationToken.None);

        //    // Проверяем результат
        //    Assert.Single(result);
        //    Assert.Equal("Test Book", result[0].Title);
        //}

        //private IServiceProvider ConfigureInMemoryServices()
        //{
        //    var services = new ServiceCollection();

        //    // Настраиваем контекст только с InMemoryDatabase
        //    services.AddDbContext<BookDbContext>(options =>
        //        options.UseInMemoryDatabase("TestDatabase")); // Никакого SqlServer!

        //    // Регистрируем BookServices
        //    services.AddScoped<BookServices>();

        //    return services.BuildServiceProvider();
        //}

        //public async Task GetAllAuthorAsyncTests()
        //{

        //}

        //public async Task GetBookByIdAsyncTests()
        //{

        //}

        //public async Task CreateBookAsyncTests()
        //{

        //}

        //public async Task UpdateBookAsyncTests()
        //{

        //}

        //public async Task DeleteBookAsyncTests()
        //{

        //}
    }
}
