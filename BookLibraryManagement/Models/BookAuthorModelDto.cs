namespace BookLibraryManagement.Models;

public record BookAuthorModelDto(string FullName, DateTime Birthday)
{
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
