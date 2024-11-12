namespace BookLibraryManagement.Models;
public class BookModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public BookAuthorModel Author { get; set; }

    public int PublishedYear { get; set; }

    public string Genre { get; set; }

    public Guid AuthorId { get; set; }

}
