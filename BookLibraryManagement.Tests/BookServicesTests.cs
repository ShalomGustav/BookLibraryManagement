using BookLibraryManagement.Models;
using BookLibraryManagement.Repositories;
using BookLibraryManagement.Services;
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
            //Arrange
            var books = new List<BookModel>
            {
                new BookModel {Id = Guid.NewGuid(), Title = "Тестовое описание для UnitTests_1"},
                new BookModel {Id = Guid.NewGuid(), Title = "Тестовое описание для UnitTests_2"}
            };

            var mockContext = new Mock<BookDbContext>();
            mockContext.Setup(x => x.Books).ReturnsDbSet(books);

            var mocDbService = new BookServices(mockContext.Object);

            //Act
            var result = await mocDbService.GetAllAsync(CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Тестовое описание для UnitTests_1", result[0].Title);
            Assert.Equal("Тестовое описание для UnitTests_2", result[1].Title);

        }

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
