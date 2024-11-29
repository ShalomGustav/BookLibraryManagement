using BookLibraryManagement.Interfaces;
using BookLibraryManagement.Models;
using BookLibraryManagement.Repositories;
using BookLibraryManagement.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace BookLibraryManagement.Tests
{
    public class BookServicesTests
    {
        private readonly Mock<IBookServices> _mockBookServices;
        private readonly Mock<BookDbContext> _mockDbContext;
        private readonly BookServices _bookServices;

        public BookServicesTests()
        {
            _mockBookServices = new Mock<IBookServices>();
            _mockDbContext = new Mock<BookDbContext>();
            _bookServices = new BookServices(_mockDbContext.Object);
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

        #region AssertHelpers
        public static void AssertBookHelper(BookModel book, string title, string genre, int publishedYear)
        {
            Assert.NotNull(book);
            Assert.NotEqual(Guid.Empty, book.Id);
            Assert.Equal(title, book.Title);
            Assert.Equal(genre, book.Genre);
            Assert.Equal(publishedYear, book.PublishedYear);
        }

        public static void AssertAuthorHelper(BookAuthorModel bookAuthorModel, string fullName, DateTime birthday)
        {
            Assert.NotNull(bookAuthorModel);
            Assert.NotEqual(Guid.Empty, bookAuthorModel.Id);
            Assert.Equal(fullName, bookAuthorModel.FullName);
            Assert.Equal(birthday, bookAuthorModel.Birthday);
        }
        #endregion


        [Fact]
        private async Task GetAllAsyncTests()
        {

            //// Arrange
            //var books = new List<BookModel> { CreateTestBook() };
            //_mockDbContext.Setup(x => x.Books).ReturnsDbSet(books);

            //// Act
            //var result = await _bookServices.GetAllAsync(CancellationToken.None);


            // Arrange
            _mockBookServices.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<BookModel> {CreateTestBook()});

            // Act
            var result = await _mockBookServices.Object.GetAllAsync(CancellationToken.None); 
            var book = result[0];

            // Assert
            Assert.Single(result);

            AssertBookHelper(book, "Title 1", "Genre 1", 2000);

            AssertAuthorHelper(book.Author, "FullName 1", new DateTime(2000, 11, 9));
        }

        [Fact]
        private async Task GetAllAuthorAsyncTests()
        {

            //// Arrange
            //var authors = new List<BookAuthorModel> { CreateTestAuthor() };
            //_mockDbContext.Setup(x => x.BookAuthor).ReturnsDbSet(authors);

            //// Act
            //var result = await _bookServices.GetAllAuthorAsync(CancellationToken.None);

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
            // // Arrange
            //var book = CreateTestBook();
            //_mockDbContext.Setup(x => x.Books).ReturnsDbSet(new List<BookModel> { book });

            //// Act
            //var result = await _bookServices.GetBookByIdAsync(book.Id, CancellationToken.None);

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

        [Fact]
        private async Task UpdateBookAsyncTests()
        {
            // Arrange
            var book = CreateTestBook(x =>
            {
                x.Title = "Old Title";    
                x.Genre = "Old Genre";   
                x.PublishedYear = 1990;  
            });

            _mockDbContext.Setup(x => x.Books).ReturnsDbSet(new List<BookModel> { book });

            var service = new BookServices(_mockDbContext.Object);

            // Act
            await service.UpdateBookAsync(book.Id, "New Title", "New Genre", 2000, CancellationToken.None);

            // Assert
            Assert.Equal("New Title", book.Title);
            Assert.Equal("New Genre", book.Genre);
            Assert.Equal(2000, book.PublishedYear);

            _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        private async Task DeleteBookTrueAsyncTests()
        {
            var book = CreateTestBook();

            _mockDbContext.Setup(x => x.Books).ReturnsDbSet(new List<BookModel> { book });

            //Act
            var result = await _bookServices.DeleteBookAsync(book.Id,CancellationToken.None);

            //Assert
            Assert.True(result);
            _mockDbContext.Verify(x => x.Books.Remove(It.Is<BookModel>(y => y.Id == book.Id)), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        private async Task DeleteBookFalseAsyncTests()
        {
            var nonExistBookId = Guid.NewGuid();

            _mockDbContext.Setup(x => x.Books).ReturnsDbSet(new List<BookModel>());

            //Act
            var result = await _bookServices.DeleteBookAsync(nonExistBookId, CancellationToken.None);

            //Assert
            Assert.False(result);
            _mockDbContext.Verify(x => x.Books.Remove(It.IsAny<BookModel>()), Times.Never);
            _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
