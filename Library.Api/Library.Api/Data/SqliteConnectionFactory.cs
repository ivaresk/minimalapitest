using Library.Api.Data;
using Microsoft.Data.Sqlite;
using System.Data;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connecion =new  SqliteConnection(this.connectionString);
        await connecion.OpenAsync();
        return connecion;
    }
}

