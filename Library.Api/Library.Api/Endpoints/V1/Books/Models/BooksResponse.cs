namespace Library.Api.Endpoints.V1.Books.Models;

public class BooksResponse
{
    public IEnumerable<BookResponse> Books { get; set; } = default!;
}
