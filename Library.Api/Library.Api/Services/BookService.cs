using Dapper;
using Library.Api.Data;
using Library.Api.Models;

namespace Library.Api.Services;

public class BookService : IBookService
{
    private readonly IDbConnectionFactory connectionFactory;

    public BookService(IDbConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(Book book)
    {
        var existinBook = await GetByIsbnAsync(book.Isbn);
        if (existinBook is not null)
        {
            return false;
        }

        var connection = await this.connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Books (Isbn, Title, Author, ShortDescription, PageCount, ReleaseDate)
              VALUES (@Isbn, @Title, @Author, @ShortDescription, @PageCount, @ReleaseDate)",
            book);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string isbn)
    {
        var connection = await this.connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"DELETE FROM Books WHERE Isbn = @Isbn", new { Isbn = isbn });
        return result > 0;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        var connection = await this.connectionFactory.CreateConnectionAsync();
        var books = await connection.QueryAsync<Book>("SELECT * FROM Books");
        return books;
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        var connection = await this.connectionFactory.CreateConnectionAsync();
        var book = connection.QuerySingleOrDefault<Book>("SELECT * FROM Books WHERE Isbn = @Isbn LIMIT 1", new { Isbn = isbn });
        return book;
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string searchTerm)
    {
        var connection = await connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<Book>
                                        ("SELECT * FROM Books WHERE Title LIKE '%' || @SearchTerm || '%'",
                                        new { SearchTerm = searchTerm });
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        var existinBook = await GetByIsbnAsync(book.Isbn);
        if (existinBook is null)
        {
            return false;
        }

        var connection = await connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"UPDATE Books SET Isbn = @Isbn, Title = @Title, Author = @Author, 
                               ShortDescription = @ShortDescription, PageCount = @PageCount, ReleaseDate = @ReleaseDate
                        WHERE Isbn = @Isbn", book);
        return result > 0;
    }
}
