using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;

namespace Library.Api.Endpoints.V1.Books.Mappers;

public static class BookMapper
{ 
    public static BookResponse ToResponse(this Book book)
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

    public static Book ToEntity(this BookRequest bookRequest)
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
