using FluentValidation.Results;
using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Services;

namespace Library.Api.Endpoints.V1.Books;

public class DeleteBookEndpoint : EndpointWithoutRequest
{
    private readonly IBookService bookService;

    public DeleteBookEndpoint(IBookService bookService)
    {
        this.bookService = bookService;
    }

    public override void Configure()
    {
        Delete("v1/books/{isbn}");
        AllowAnonymous();
        Description(b => b
          .WithName("DeleteBook")
          .Produces(204)
          .Produces(404)
          .WithTags("Books"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var isbn = Route<string>("isbn");
        var deleted = await bookService.DeleteAsync(isbn);
        if (deleted)
        {
            await SendNoContentAsync();
            return;
        }
        
        await SendNotFoundAsync();
    }
}
