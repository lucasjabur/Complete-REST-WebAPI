using Microsoft.EntityFrameworkCore;
using REST_WebAPI.Models;
using REST_WebAPI.Repositories;

namespace REST_WebAPI.Services.Implementations {
    public class BookServicesImpl : IBookServices {
        private readonly IBookRepository _repository;

        public BookServicesImpl(IBookRepository repository) {
            _repository = repository;
        }

        public Book Create(Book book) {
            return _repository.Create(book);
        }
        public Book Update(Book book) {
            return _repository.Update(book);
        }

        public void Delete(long id) {
            _repository.Delete(id);
        }

        public Book FindById(long id) {
            return _repository.FindById(id);
        }

        public List<Book> FindAll() {
            return _repository.FindAll();
        }

    }
}
