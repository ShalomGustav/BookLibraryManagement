namespace BookLibraryManagement.Models;

public class BookAuthorModelDto
{
    public string FullName { get; set; }

    public DateTime Birthday { get; set; }

    public static BookAuthorModel CreateAuthor(BookAuthorModelDto bookAuthor)
    {
        var result = new BookAuthorModel 
        { 
            FullName = bookAuthor.FullName,
            Birthday = bookAuthor.Birthday,
        };
        return result;
    }
}
