using Library.Api.Endpoints.V1.Books.Mappers;
using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;
using Library.Api.Services;

namespace Library.Api.Endpoints.V1.Books;
public class GetBooksEndpoint : EndpointWithoutRequest<List<BookResponse>>
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
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var matchedBooks = await bookService.SearchByTitleAsync(searchTerm);

            var books = matchedBooks.Select(b => b.FromEntity()).ToList();
            await SendAsync(books);
            return;
        }

        var allBooks = await bookService.GetAllAsync();

        await SendAsync(allBooks.Select(b => b.FromEntity()).ToList());
    }
}
