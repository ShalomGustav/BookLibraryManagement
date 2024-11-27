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
        #region ModelsOnTests
        private BookModel CreateTestBook(Action<BookModel> configure = null)
        {
            var author = new BookAuthorModel
            {
                Id = Guid.NewGuid(),
                FullName = "FullName 1",
                Birthday = new DateTime(2000, 11, 9)
            };

            var book = new BookModel
            {
                Id = Guid.NewGuid(),
                Title = "Title 1",
                Genre = "Genre 1",
                PublishedYear = 2000,
                Author = author,
                AuthorId = author.Id
            };

            configure?.Invoke(book);
            return book;
        }

        private BookAuthorModel CreateTestAuthor(Action<BookAuthorModel> configure = null)
        {
            var author = new BookAuthorModel
            {
                Id = Guid.NewGuid(),
                FullName = "FullName 1",
                Birthday = new DateTime(2000, 11, 9),
            };

            var book = new BookModel
            {
                Id = Guid.NewGuid(),
                Title = "Title 1",
                Genre = "Genre 1",
                PublishedYear = 2000,
                AuthorId = author.Id, 
                Author = author 
            };

            author.Book = book;

            configure?.Invoke(author);
            return author;
        }
        #endregion


        [Fact]
        private async Task GetAllAsyncTests()
        {
            // Arrange
            _mockBookServices.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookModel> {CreateTestBook()});
            // Act
            var result = await _mockBookServices.Object.GetAllAsync(CancellationToken.None); // CancellationToken.None указывает, что отмена вызова не предполагается.
            var book = result[0];
            // Assert
            Assert.NotNull(result); 
            Assert.Single(result);

            Assert.NotEqual(Guid.Empty, book.Id);
            Assert.Equal("Title 1", book.Title);
            Assert.Equal("Genre 1", book.Genre);
            Assert.Equal(2000, book.PublishedYear);

            Assert.NotNull(book.Author);
            Assert.NotEqual(Guid.Empty, book.Author.Id);
            Assert.Equal("FullName 1", book.Author.FullName);
            Assert.Equal(new DateTime(2000, 11, 9), book.Author.Birthday);
        }

        [Fact]
        private async Task GetAllAuthorAsyncTests()
        {
            //Arrange
            _mockBookServices.Setup(x => x.GetAllAuthorAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookAuthorModel> 
                {
                    CreateTestAuthor()
                });

            //Act
            var result = await _mockBookServices.Object.GetAllAuthorAsync(CancellationToken.None);
            var book = result[0];
            //Assert
            Assert.NotNull(result);
            Assert.Single(result);

            Assert.NotEqual(Guid.Empty, book.Id);
            Assert.Equal("FullName 1", book.FullName);
            Assert.Equal(new DateTime(2000, 11, 9), book.Birthday);

            Assert.NotNull(book.Book);
            Assert.NotEqual(Guid.Empty, book.Book.Id);
            Assert.Equal("Title 1", book.Book.Title);
            Assert.Equal(2000, book.Book.PublishedYear);
            Assert.Equal("Genre 1", book.Book.Genre);
            Assert.Equal(book.Id, book.Book.AuthorId);
        }

        [Fact]
        private async Task GetBookByIdAsyncTests()
        {
            var testBook = CreateTestBook();
            //Arrange 
            _mockBookServices.Setup(x => x.GetBookByIdAsync(
                It.Is<Guid>(x => x == testBook.Id),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(testBook);
            //Act
            var result = await _mockBookServices.Object.GetBookByIdAsync(testBook.Id, CancellationToken.None);
            //Assert
            Assert.NotNull(result);

            Assert.Equal(testBook.Id, result.Id);
            Assert.Equal("Title 1", result.Title);
            Assert.Equal("Genre 1", result.Genre);
            Assert.Equal(2000, result.PublishedYear);

            Assert.NotNull(result.Author);
            Assert.Equal(testBook.Author.Id, result.Author.Id);
            Assert.Equal("FullName 1", result.Author.FullName);
            Assert.Equal(new DateTime(2000, 11, 9), result.Author.Birthday);
        }

        [Fact]
        private async Task CreateBookAsync()
        {
            var bookModel = CreateTestBook();
            //Arrange
            _mockBookServices.Setup(x => x.CreateBookAsync(
                It.Is<BookModel>(x => x.Id == bookModel.Id),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookModel);

            //Act
            var result = await _mockBookServices.Object.CreateBookAsync(bookModel, CancellationToken.None);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(bookModel.Id, result.Id);
            Assert.Equal(bookModel.AuthorId, result.AuthorId);
            Assert.Equal("Genre 1", result.Genre);
            Assert.Equal(2000, result.PublishedYear);
            Assert.Equal("Title 1", result.Title);

            Assert.NotNull(result.Author);
            Assert.Equal(bookModel.Author.Id, result.Author.Id);
            Assert.Equal(bookModel.Author.Birthday, result.Author.Birthday);
            Assert.Equal(bookModel.Author.FullName, result.Author.FullName);
        }
    }
}
