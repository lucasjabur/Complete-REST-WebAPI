using REST_WebAPI.Models;

namespace REST_WebAPI.Services {
    public interface IBookServices {
        Book Create(Book book);
        Book FindById(long id);
        List<Book> FindAll();
        Book Update(Book book);
        void Delete(long id);
    }
}
