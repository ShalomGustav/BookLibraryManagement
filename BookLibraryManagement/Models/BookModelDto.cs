using System.ComponentModel.DataAnnotations;

namespace BookLibraryManagement.Models;

public class BookModelDto
{
    [StringLength(32)]
    public string Title { get; set; }

    public BookAuthorModelDto Author { get; set; }

    [Range(1000, 9999)]
    public int PublishedYear { get; set; }

    [StringLength(32)]
    public string Genre { get; set; }

    public static BookModel CreateBook(BookModelDto book)
    {
        var result = new BookModel
        {
            Title = book.Title,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear,
            Author = BookAuthorModelDto.CreateAuthor(book.Author)
        };

        return result;
    }
}
