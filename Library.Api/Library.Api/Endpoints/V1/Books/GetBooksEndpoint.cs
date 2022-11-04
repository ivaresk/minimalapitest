namespace Library.Api.Endpoints.V1.Books;

using Library.Api.Endpoints.V1.Books.Mappers;
using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;
using Library.Api.Services;

public class GetBooksEndpoint : EndpointWithoutRequest<List<BookResponse>>
{

    private readonly IBookService bookService;

    public GetBooksEndpoint(IBookService bookService)
    {
        this.bookService = bookService;
    }

    public override void Configure()
    {
        Get("v1/books");
        AllowAnonymous();
        Description(b => b
                  .WithName("GetBooks")
                  .Produces<IEnumerable<Book>>(200)
                  .WithTags("Books"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var searchTerm = Query<string>("searchTerm", isRequired: false);
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var matchedBooks = await bookService.SearchByTitleAsync(searchTerm);

            var books = matchedBooks.Select(b => b.ToResponse()).ToList();
            await SendAsync(books);
            return;
        }

        var allBooks = await bookService.GetAllAsync();

        await SendAsync(allBooks.Select(b => b.ToResponse()).ToList());
    }
}
