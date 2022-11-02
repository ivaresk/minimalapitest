using Library.Api.Endpoints.V1.Books.Mappers;
using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;
using Library.Api.Services;

namespace Library.Api.Endpoints.V1.Books.GetBooks;
public class GetBooksEndpoint : EndpointWithoutRequest<BooksResponse, BookResponseMapper>
{
    public override void Configure()
    {
        Get("v1/books");
        AllowAnonymous();
        Description(b => b
                  .WithName("GetBooks")
                  .Produces<IEnumerable<Book>>(200)
                  .WithTags("Books"));
    }

    private readonly IBookService bookService;

    public GetBooksEndpoint(IBookService bookService)
    {
        this.bookService = bookService;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var searchTerm = Query<string>("searchTerm", isRequired: false);
        BooksResponse booksResponse;
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var matchedBooks = await bookService.SearchByTitleAsync(searchTerm);
            booksResponse = new BooksResponse
            {
                Books = matchedBooks.Select(Map.FromEntity)
            };
            await SendAsync(booksResponse);
            return;
        }

        var books = await bookService.GetAllAsync();

        booksResponse = new BooksResponse
        {
            Books = books.Select(Map.FromEntity)
        };
        await SendAsync(booksResponse);
    }
}
