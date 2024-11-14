namespace BookLibraryManagement.Models;

public class BookModelDto
{
    public string Title { get; set; }

    public BookAuthorModelDto Author { get; set; }

    public int PublishedYear { get; set; }

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
