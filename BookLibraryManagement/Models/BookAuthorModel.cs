using System.Text.Json.Serialization;

namespace BookLibraryManagement.Models;

public class BookAuthorModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public DateTime Birthday { get; set; }

    [JsonIgnore]
    public BookModel Book { get; set; }
}
