using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;

namespace Library.Api.Endpoints.V1.Books.Mappers;

public class BookResponseMapper : ResponseMapper<BookResponse, Book>
{
    public override BookResponse FromEntity(Book book)
    {
        return new BookResponse
        {
            Author = book.Author,
            Isbn = book.Isbn,
            PageCount = book.PageCount,
            ReleaseDate = book.ReleaseDate,
            ShortDescription = book.ShortDescription,
            Title = book.Title
        };
    }
}
