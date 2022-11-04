namespace Library.Api.Endpoints.V1.Books;

using FluentValidation.Results;
using Library.Api.Endpoints.V1.Books.Mappers;
using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;
using Library.Api.Services;

public class UpdateBookEndpoint : Endpoint <BookRequest, BookResponse>
{
    private readonly IBookService bookService;

    public UpdateBookEndpoint(IBookService bookService)
    {
        this.bookService = bookService;
    }

    public override void Configure()
    {
        Put("v1/books/{isbn}");
        AllowAnonymous();
        Description(b => b
                  .WithName("UpdateBook")
                  .Accepts<Book>("application/json")
                  .Produces<Book>(200)
                  .Produces<IEnumerable<ValidationFailure>>(400)
                  .WithTags("Books"));
    }

    public override async Task HandleAsync(BookRequest req, CancellationToken ct)
    {
        var book = req.ToEntity();
        var updated = await bookService.UpdateAsync(book);
        if (updated)
        {
            await SendAsync(book.ToResponse());
            return;

        }

        await SendNotFoundAsync();
    }
}
