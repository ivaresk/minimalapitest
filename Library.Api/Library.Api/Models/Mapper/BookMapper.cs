using Library.Api.Endpoints.V1.Books.Models;

namespace Library.Api.Models.Mapper;

public static class BookMapper
{
    public static Book ToModel(this BookRequest bookRequest)
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
