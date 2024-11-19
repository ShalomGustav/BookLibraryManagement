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

    public async Task<List<BookModel>> GetAllAsync(CancellationToken ctx)//
    {
        var result = await _dbContext.Books
            .Include(x => x.Author)
            .ToListAsync(ctx);

        return result;
    }

    public async Task<List<BookAuthorModel>> GetAllAuthorAsync(CancellationToken ctx)//
    {
        var result = await _dbContext.BookAuthor
            .ToListAsync(ctx);

        return result;
    }

    public async Task<BookModel> GetBookByIdAsync(Guid id,CancellationToken ctx)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var result = await _dbContext.Books
            .Include(x => x.Author)
            .FirstOrDefaultAsync(y => y.Id == id, ctx);
        return result;
    }

    public async Task<BookModel> CreateBookAsync(BookModel bookModel, CancellationToken ctx)
    {
        await _dbContext.Books.AddAsync(bookModel,ctx);
        await _dbContext.SaveChangesAsync(ctx);    

        return bookModel;
    }
    
    public async Task UpdateBookAsync(Guid id, string title, string genre, int publishedYear, CancellationToken ctx)
    {
        var existBook = await _dbContext.Books.FirstOrDefaultAsync(y => y.Id == id, ctx);

        if (existBook != null)
        {
            existBook.Title = title;
            existBook.Genre = genre;  
            existBook.PublishedYear = publishedYear;

            await _dbContext.SaveChangesAsync(ctx);
        }
    }

    public async Task<bool> DeleteBookAsync(Guid id, CancellationToken ctx)
    {
        var existBook = await _dbContext.Books
            .Include(x => x.Author)
            .FirstOrDefaultAsync(y => y.Id == id, ctx);

        if (existBook != null)
        {
            _dbContext.Books.Remove(existBook);
            await _dbContext.SaveChangesAsync(ctx);
            return true;
        }
        return false;
    }
}
