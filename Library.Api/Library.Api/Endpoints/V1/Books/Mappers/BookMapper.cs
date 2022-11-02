using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;

namespace Library.Api.Endpoints.V1.Books.Mappers;

public class BookMapper : Mapper<BookRequest, BookResponse, Book>
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

    public override Book ToEntity(BookRequest bookRequest)
    {
        return new Book
        {
            Author = bookRequest.Author,
            Isbn = bookRequest.Isbn,
            PageCount = bookRequest.PageCount,
            ReleaseDate = bookRequest.ReleaseDate,
            ShortDescription = bookRequest.ShortDescription,
            Title = bookRequest.Title
        };
    }
}
