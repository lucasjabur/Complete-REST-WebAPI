using REST_WebAPI.Models;

namespace REST_WebAPI.Repositories {
    public interface IBookRepository : IRepository<Book> {
        PagedSearch<Book> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page);
    }
}
