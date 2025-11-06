using Mapster;
using Microsoft.EntityFrameworkCore;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Models;
using REST_WebAPI.Repositories;

namespace REST_WebAPI.Services.Implementations {
    public class BookServicesImpl : IBookServices {

        private readonly IRepository<Book> _repository;

        public BookServicesImpl(IRepository<Book> repository) {
            _repository = repository;
        }

        public BookDTO Create(BookDTO book) {
            var entity = book.Adapt<Book>();
            entity = _repository.Create(entity);
            return entity.Adapt<BookDTO>();
        }

        public BookDTO Update(BookDTO book) {
            var entity = book.Adapt<Book>();
            entity = _repository.Update(entity);
            return entity.Adapt<BookDTO>();
        }

        public void Delete(long id) {
            _repository.Delete(id);
        }

        public BookDTO FindById(long id) {
            return _repository.FindById(id).Adapt<BookDTO>();
        }

        public List<BookDTO> FindAll() {
            return _repository.FindAll().Adapt<List<BookDTO>>();
        }

    }
}
