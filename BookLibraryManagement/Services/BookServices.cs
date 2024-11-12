using BookLibraryManagement.Models;
using BookLibraryManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryManagement.Services;

public class BookServices 
{
    private readonly BookDbContext _dbContext;

    public BookServices(BookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BookModel>> GetAllAsync()//
    {
        var result = await _dbContext.Books
            .Include(x => x.Author)
            .ToListAsync();

        return result;
    }

    public async Task<List<BookAuthorModel>> GetAllAuthorAsync()//
    {
        var result = await _dbContext.BookAuthor
            .ToListAsync();

        return result;
    }

    public async Task<BookModel> GetBookByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var result = await _dbContext.Books
            .Include(x => x.Author)
            .FirstOrDefaultAsync(y => y.Id == id);
        return result;
    }

    public async Task<BookModel> CreateBookAsync(BookModel bookModel)
    {
        await _dbContext.Books.AddAsync(bookModel);
        await _dbContext.SaveChangesAsync();    

        return bookModel;
    }
    
    public async Task UpdateBookAsync(Guid id, string title, string genre, int publishedYear)
    {
        var existBook = await _dbContext.Books.FirstOrDefaultAsync(y => y.Id == id);

        if (existBook != null)
        {
            existBook.Title = title;
            existBook.Genre = genre;  
            existBook.PublishedYear = publishedYear;

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteBookAsync(Guid id)
    {
        var existBook = await _dbContext.Books
            .Include(x => x.Author)
            .FirstOrDefaultAsync(y => y.Id == id);

        if (existBook != null)
        {
            _dbContext.Books.Remove(existBook);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
