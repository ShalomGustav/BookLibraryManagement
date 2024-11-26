using BookLibraryManagement.Interfaces;
using BookLibraryManagement.Models;
using Moq;

namespace BookLibraryManagement.Tests
{
    public class BookServicesTests
    {
        [Fact]
        public async Task GetAllAsyncTests()
        {
            // Arrange: подготовка теста
            var mockBookService = new Mock<IBookServices>();

            mockBookService.Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookModel>
                {                
                    new BookModel
                    {
                        Id = Guid.NewGuid(), 
                        Title = "Book 1", 
                        Author = new BookAuthorModel
                        {
                            Id = Guid.NewGuid(),
                            FullName = "Author 1" ,
                            Birthday = new DateTime(2010,11,9)
                        },
                        PublishedYear = 1995,
                        Genre = "Genre 1",
                    },
                });

            // Act: выполнение тестируемого метода
            var result = await mockBookService.Object.GetAllAsync(CancellationToken.None);
            // Вызываем метод GetAllAsync у настроенного мока.
            // CancellationToken.None указывает, что отмена вызова не предполагается.

            // Assert: проверка результатов
            Assert.NotNull(result); 
            Assert.Single(result);
            Assert.Equal("Book 1", result[0].Title);
            Assert.Equal("Genre 1", result[0].Genre);
            Assert.Equal(1995, result[0].PublishedYear);

            Assert.NotNull(result[0].Author);
            Assert.Equal("Author 1", result[0].Author.FullName);
            Assert.Equal(new DateTime(2010,11,9), result[0].Author.Birthday);
        }
    }
}
