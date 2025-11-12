using Mapster;
using REST_WebAPI.Hypermedia.Utils;
using REST_WebAPI.Models;
using REST_WebAPI.Models.Context;
using REST_WebAPI.Repositories.QueryBuilders;

namespace REST_WebAPI.Repositories.Implementations {
    public class BookRepository(MSSQLContext context)
        : GenericRepository<Book>(context), IBookRepository {

        public PagedSearch<Book> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page) {
            var queryBuilder = new PersonQueryBuilder();
            var (query, countQuery, sort, size, offset) = queryBuilder.BuildQueries(title, sortDirection, pageSize, page);

            var books = base.FindWithPagedSearch(query);
            var totalResults = base.GetCount(countQuery);

            return new PagedSearch<Book> {
                CurrentPage = page,
                List = books,
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }
    }
}
