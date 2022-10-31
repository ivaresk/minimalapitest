namespace Library.Api.Endpoints.V1.Books.CreateBook;

using FastEndpoints;
using FluentValidation.Results;
using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Services;
using System.Threading;
using System.Threading.Tasks;

public class Endpoint : Endpoint<BookRequest, BookResponse>
{
    public override void Configure()
    {
        Post("v1/books");
        AllowAnonymous();
        Description(b => b
          .WithName("CreateBook")
          .Accepts<BookRequest>("application/json")
          .Produces<BookResponse>(201)
          .Produces<IEnumerable<ValidationFailure>>(400)
          .WithTags("Books"));
    }

    private readonly IBookService bookService;

    public Endpoint(IBookService bookService)
    {
        this.bookService = bookService;
    }

    public override async Task HandleAsync(BookRequest req, CancellationToken ct)
    {
        var created = await bookService.CreateAsync(req);
        if (!created)
        {
            AddError(f => f.Isbn, "A book with this Isbn Already exist");
            ThrowIfAnyErrors();
        }

        await SendAsync(new BookResponse
        {
            Isbn = req.Isbn,
            Author = req.Author,
            PageCount = req.PageCount,
            ReleaseDate = req.ReleaseDate,
            ShortDescription = req.ShortDescription,
            Title = req.Title,
        });
    }
}
