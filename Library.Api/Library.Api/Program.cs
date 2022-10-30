using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using Library.Api.Auth;
using Library.Api.Data;
using Library.Api.Models;
using Library.Api.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AnyOrigin", policy => policy.AllowAnyOrigin());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAuthentication(ApiKeySchemeConstants.SchemeName)
                .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeySchemeConstants.SchemeName, _ => { });
builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();
var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseFastEndpoints();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();

app.MapPost("books",
    [Authorize(AuthenticationSchemes = ApiKeySchemeConstants.SchemeName)]
    async (Book book, IBookService bookService, IValidator<Book> validator) =>
{
    var validationResult = await validator.ValidateAsync(book);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    var created = await bookService.CreateAsync(book);
    if (!created)
    {
        return Results.BadRequest(new List<ValidationFailure>
        {
            new("isbn","A book with this Isbn Already exist" )
        });
    }

    //return Results.Created($"/books/{book.Isbn}", book);
    return Results.CreatedAtRoute("GetBook", new { isbn = book.Isbn }, book);
}).WithName("CreateBook")
  .Accepts<Book>("application/json")
  .Produces<Book>(201)
  .Produces<IEnumerable<ValidationFailure>>(400)
  .WithTags("Books");

app.MapGet("books", async (IBookService bookService, string? searchTerm) =>
{
    if (!string.IsNullOrEmpty(searchTerm))
    {
        var matchedBooks = await bookService.SearchByTitleAsync(searchTerm);
        return Results.Ok(matchedBooks);
    }

    var books = await bookService.GetAllAsync();
    return Results.Ok(books);
}).WithName("GetBooks")
  .Produces<IEnumerable<Book>>(200)
  .WithTags("Books");

app.MapGet("books/{isbn}", async (string isbn, IBookService bookService) =>
{
    var book = await bookService.GetByIsbnAsync(isbn);
    return book is not null ? Results.Ok(book) : Results.NotFound();
}).WithName("GetBook")
  .Produces<Book>(200)
  .Produces(204)
  .WithTags("Books");

app.MapPut("books/{isbn}", async (string isbn, Book book, IBookService bookService, IValidator<Book> validator) =>
{
    var validationResult = await validator.ValidateAsync(book);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    var updated = await bookService.UpdateAsync(book);
    return updated ? Results.Ok(book) : Results.NotFound();

}).WithName("UpdateBook")
  .Accepts<Book>("application/json")
  .Produces<Book>(200)
  .Produces<IEnumerable<ValidationFailure>>(400)
  .WithTags("Books");

app.MapDelete("books/{isbn}", async (string isbn, IBookService bookService) =>
{
    var deleted = await bookService.DeleteAsync(isbn);
    return deleted ? Results.NoContent() : Results.NotFound();
}).WithName("DeleteBook")
  .Produces(204)
  .Produces(404)
  .WithTags("Books");

app.Run();