using BookLibraryManagement.Models;

namespace BookLibraryManagement.Interfaces
{
    public interface IBookServices
    {
        Task<List<BookModel>> GetAllAsync(CancellationToken ctx);
        Task<List<BookAuthorModel>> GetAllAuthorAsync(CancellationToken ctx);
        Task<BookModel> GetBookByIdAsync(Guid id, CancellationToken ctx);
        Task<BookModel> CreateBookAsync(BookModel bookModel, CancellationToken ctx);
        Task UpdateBookAsync(Guid id, string title, string genre, int publishedYear, CancellationToken ctx);
        Task<bool> DeleteBookAsync(Guid id, CancellationToken ctx);
    }
}
