namespace Library.Api.Endpoints.V1.Books;

using Library.Api.Endpoints.V1.Books.Mappers;
using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;
using Library.Api.Services;
using System.Threading;
using System.Threading.Tasks;

public class GetByIsbnEndpoint : EndpointWithoutRequest<BookResponse>
{
    private readonly IBookService bookService;

    public GetByIsbnEndpoint(IBookService bookService)
    {
        this.bookService = bookService;
    }

    public override void Configure()
    {
        Get("v1/books/{isbn}");
        AllowAnonymous();
        Description(b => b
                  .WithName("GetBookByIsbn")
                  .Produces<Book>(200)
                  .Produces(204)
                  .WithTags("Books"));
    }



    public override async Task HandleAsync(CancellationToken ct)
    {
        var isbn = Route<string>("isbn");
        var book = await bookService.GetByIsbnAsync(isbn);
        if (book is not null)
        {
            await SendAsync(book.ToResponse());
            return;
        }

        await SendNotFoundAsync();
    }
}
