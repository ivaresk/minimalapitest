namespace Library.Api.Services;

using Library.Api.Endpoints.V1.Books.Models;
using Library.Api.Models;

public interface IBookService
{
    public Task<bool> CreateAsync(Book book);
    public Task<Book?> GetByIsbnAsync(string isbn);
    public Task<IEnumerable<Book>> GetAllAsync();
    public Task<IEnumerable<Book>> SearchByTitleAsync(string searchTerm);
    public Task<bool> UpdateAsync(Book book);
    public Task<bool> DeleteAsync(string isbn);
}
