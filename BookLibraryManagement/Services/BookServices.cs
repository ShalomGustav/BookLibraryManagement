using BookLibraryManagement.Interfaces;
using BookLibraryManagement.Models;
using BookLibraryManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BookLibraryManagement.Services;

public class BookServices : IBookServices
{
    private readonly BookDbContext _dbContext;
    private readonly CacheSettings _cacheSettings;
    private readonly IMemoryCache _cache;

    private const string KeyBook = "AllBooks";
    private const string KeyAuthor = "AllAuthors";

    public BookServices(BookDbContext dbContext, IOptions<CacheSettings> cacheSettings, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cacheSettings = cacheSettings.Value;
        _cache = cache;
    }

    public async Task<List<BookModel>> GetAllAsync(CancellationToken ctx)
    {
        if (!_cache.TryGetValue(KeyBook, out List<BookModel> result))
        {
            result = await _dbContext.Books
                .Include(x => x.Author)
                .ToListAsync(ctx);

            _cache.Set(KeyAuthor, result, GetCacheOptions());
        }
        return result;
    }

    public async Task<List<BookAuthorModel>> GetAllAuthorAsync(CancellationToken ctx)
    {
        if(!_cache.TryGetValue(KeyBook, out List<BookAuthorModel> result))
        {
            result = await _dbContext.BookAuthor
                .ToListAsync(ctx);
            
            _cache.Set(KeyAuthor, result, GetCacheOptions());
        }
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

        ResetCache();

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

        ResetCache();
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

            ResetCache();
            return true;
        }
        return false;
    }

    private void ResetCache()
    {
        _cache.Remove(KeyBook);
        _cache.Remove(KeyAuthor);
    }

    private MemoryCacheEntryOptions GetCacheOptions() => new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.ExpireTime))
                .SetAbsoluteExpiration(TimeSpan.FromHours(_cacheSettings.AbsoluteExpireTime));
}
