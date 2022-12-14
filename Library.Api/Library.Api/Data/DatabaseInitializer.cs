using Dapper;

namespace Library.Api.Data;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        this.connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await this.connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS Books (
              Isbn TEXT PRIMARY KEY,
              Title TEXT NOT NULL,
              Author TEXT NOT NULL,
              ShortDescription TEXT NOT NULL,
              PageCount INTEGER,
              ReleaseDate TEXT NOT NULL)");
    }
}
