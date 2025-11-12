using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Hypermedia.Utils;

namespace REST_WebAPI.Services {
    public interface IBookServices {
        BookDTO Create(BookDTO book);
        BookDTO FindById(long id);
        List<BookDTO> FindAll();
        BookDTO Update(BookDTO book);
        void Delete(long id);
        PagedSearchDTO<BookDTO> FindWithPagedSearch(string title, string sortDirection, int pageSize, int page);

    }
}
