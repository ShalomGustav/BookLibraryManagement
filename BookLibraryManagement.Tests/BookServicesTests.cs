using BookLibraryManagement.Interfaces;
using BookLibraryManagement.Models;
using Moq;

namespace BookLibraryManagement.Tests
{
    public class BookServicesTests
    {
        private readonly Mock<IBookServices> _mockBookServices;

        public BookServicesTests()
        {
            _mockBookServices = new Mock<IBookServices>();
        }

        [Fact]
        private async Task GetAllAsyncTests()
        {
            // Arrange
            _mockBookServices.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
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

            // Act
            var result = await _mockBookServices.Object.GetAllAsync(CancellationToken.None); // CancellationToken.None указывает, что отмена вызова не предполагается.
            var book = result[0];
            // Assert
            Assert.NotNull(result); 
            Assert.Single(result);

            Assert.NotEqual(Guid.Empty, book.Id);
            Assert.Equal("Book 1", book.Title);
            Assert.Equal("Genre 1", book.Genre);
            Assert.Equal(1995, book.PublishedYear);

            Assert.NotNull(book.Author);
            Assert.NotEqual(Guid.Empty, book.Author.Id);
            Assert.Equal("Author 1", book.Author.FullName);
            Assert.Equal(new DateTime(2010,11,9), book.Author.Birthday);
        }

        [Fact]
        private async Task GetAllAuthorAsyncTests()
        {
            //Arrange
            _mockBookServices.Setup(x => x.GetAllAuthorAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookAuthorModel> 
                {
                    new BookAuthorModel
                    {
                        Id = Guid.NewGuid(),
                        FullName = "FullName 1",
                        Birthday = new DateTime(2010,11,9),

                        Book = new BookModel
                        {
                            Id = Guid.NewGuid(),
                            Title = "Title 1",
                            PublishedYear = 2010,
                            Genre = "Genre 1",
                            AuthorId = Guid.NewGuid()
                        },
                    }
                });

            //Act
            var result = await _mockBookServices.Object.GetAllAuthorAsync(CancellationToken.None);
            var book = result[0];
            //Assert
            Assert.NotNull(result);
            Assert.Single(result);

            Assert.NotEqual(Guid.Empty, book.Id);
            Assert.Equal("FullName 1", book.FullName);
            Assert.Equal(new DateTime(2010, 11, 9), book.Birthday);

            Assert.NotNull(book.Book);
            Assert.NotEqual(Guid.Empty, book.Book.Id);
            Assert.Equal("Title 1", book.Book.Title);
            Assert.Equal(2010, book.Book.PublishedYear);
            Assert.Equal("Genre 1", book.Book.Genre);
            Assert.NotEqual(Guid.Empty, book.Book.AuthorId);
        }

        [Fact]
        private async Task GetBookByIdAsyncTests()
        {
            var TestId = Guid.NewGuid();
            //Arrange 
            _mockBookServices.Setup(x => x.GetBookByIdAsync(
                It.Is<Guid>(x => x == TestId),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BookModel
                {
                    Id = TestId,
                    Title = "Title 1",
                    Genre = "Genre 1",
                    PublishedYear = 2010,
                    Author = new BookAuthorModel
                    {
                        Id = Guid.NewGuid(),
                        FullName = "FullName 1",
                        Birthday = new DateTime(2010, 11, 9)
                    }
                });
            //Act
            var result = await _mockBookServices.Object.GetBookByIdAsync(TestId,CancellationToken.None);
            //Assert
            Assert.NotNull(result);

            Assert.Equal(TestId, result.Id);
            Assert.Equal("Title 1", result.Title);
            Assert.Equal("Genre 1", result.Genre);
            Assert.Equal(2010, result.PublishedYear);

            Assert.NotNull(result.Author);
            Assert.NotEqual(Guid.Empty,result.Author.Id);
            Assert.Equal("FullName 1", result.Author.FullName);
            Assert.Equal(new DateTime(2010, 11, 9), result.Author.Birthday);
        }
    }
}
