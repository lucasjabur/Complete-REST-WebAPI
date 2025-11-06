using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Services {
    public interface IBookServices {
        BookDTO Create(BookDTO book);
        BookDTO FindById(long id);
        List<BookDTO> FindAll();
        BookDTO Update(BookDTO book);
        void Delete(long id);
    }
}
