using REST_WebAPI.Models;
using REST_WebAPI.Models.Context;

namespace REST_WebAPI.Repositories.Implementations {
    public class BookRepositoryImpl : IBookRepository {
        private MSSQLContext _context;

        public BookRepositoryImpl(MSSQLContext context) {
            _context = context;
        }

        public Book Create(Book book) {
            _context.Add(book);
            _context.SaveChanges();

            return book;
        }
        public Book Update(Book book) {
            var existingBook = _context.Books.Find(book.Id);

            if (existingBook == null) {
                return null;
            }

            _context.Entry(existingBook).CurrentValues.SetValues(book);
            _context.SaveChanges();

            return book;
        }

        public void Delete(long id) {
            var existingBook = _context.Books.Find(id);

            if (existingBook == null) {
                return;
            }

            _context.Remove(existingBook);
            _context.SaveChanges();
        }

        public Book FindById(long id) {
            return _context.Books.Find(id);
        }

        public List<Book> FindAll() {
            return _context.Books.ToList();
        }
    }
}
