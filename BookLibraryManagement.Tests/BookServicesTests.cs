using BookLibraryManagement.Models;
using BookLibraryManagement.Repositories;
using BookLibraryManagement.Services;
using Moq;
using Moq.EntityFrameworkCore;
using System;

namespace BookLibraryManagement.Tests
{
    public class BookServicesTests
    {
        private readonly Mock<BookDbContext> _mockDbContext;
        private readonly BookServices _bookServices;

        public BookServicesTests()
        {
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

        #region Tests
        [Fact]
        private async Task GetAllAsyncTests()
        {
            // Arrange
            var books = new List<BookModel> { CreateTestBook() };
            _mockDbContext.Setup(x => x.Books).ReturnsDbSet(books);

            // Act
            var result = await _bookServices.GetAllAsync(CancellationToken.None);
            var book = result.FirstOrDefault();

            // Assert
            Assert.Single(result);

            AssertBookHelper(book, "Title 1", "Genre 1", 2000);
            AssertAuthorHelper(book.Author, "FullName 1", new DateTime(2000, 11, 9));
        }

        [Fact]
        private async Task GetAllAuthorAsyncTests()
        {
            // Arrange
            var authors = new List<BookAuthorModel> { CreateTestAuthor() };
            var book = CreateTestBook();

            _mockDbContext.Setup(x => x.BookAuthor).ReturnsDbSet(authors);

            // Act
            var result = await _bookServices.GetAllAuthorAsync(CancellationToken.None);
            var actualAuthor = result.FirstOrDefault();

            //Assert
            Assert.Single(result);

            AssertBookHelper(book, "Title 1", "Genre 1", 2000);
            AssertAuthorHelper(actualAuthor, "FullName 1", new DateTime(2000, 11, 9));
        }

        [Fact]
        private async Task GetBookByIdAsyncTests()
        {
            // Arrange
            var book = CreateTestBook();
            var author = CreateTestAuthor();
            _mockDbContext.Setup(x => x.Books).ReturnsDbSet(new List<BookModel> { book });

            // Act
            var result = await _bookServices.GetBookByIdAsync(book.Id, CancellationToken.None);

            //Assert
            AssertBookHelper(book, "Title 1", "Genre 1", 2000);
            AssertAuthorHelper(author, "FullName 1", new DateTime(2000, 11, 9));
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
            AssertBookHelper(book, "New Title", "New Genre", 2000);

            _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("EBAC0C79-F398-4A29-8C79-08DCFB4023F4", "EBAC0C79-F398-4A29-8C79-08DCFB4023F4", true)]
        [InlineData("EBAC0C79-F398-4A29-8C79-08DCFB4023F4", "EBAC0C79-F398-4A29-8C79-08DCFB4023F5", false, 1)]
        private async Task DeleteBookTrueAsyncTests(Guid createdId, Guid expectedId, bool succesed, int? count = null)
        {
            //Arrange

            var book = CreateTestBook(x =>
            {
                x.Id = createdId;
            });

            _mockDbContext.Setup(x => x.Books).ReturnsDbSet(new List<BookModel> { book });

            //Act
            var result = await _bookServices.DeleteBookAsync(expectedId, CancellationToken.None);
            var items = await _bookServices.GetAllAsync(CancellationToken.None);

            //Assert

            Assert.Equal(succesed, result);

            if(count != null)
            {
                Assert.Equal(count, items.Count);
            }
        }
        #endregion
    }
}
