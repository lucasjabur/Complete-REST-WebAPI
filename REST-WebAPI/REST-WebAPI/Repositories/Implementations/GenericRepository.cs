using Microsoft.EntityFrameworkCore;
using REST_WebAPI.Models.Base;
using REST_WebAPI.Models.Context;

namespace REST_WebAPI.Repositories.Implementations {
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity {

        private MSSQLContext _context;
        private DbSet<T> _dataSet;

        public GenericRepository(MSSQLContext context) {
            _context = context;
            _dataSet = context.Set<T>();
        }

        public T Create(T item) {
            _context.Add(item);
            _context.SaveChanges();

            return item;
        }
        public T Update(T item) {
            var existingItem = _dataSet.Find(item.Id);

            if (existingItem == null) {
                return null;
            }

            _context.Entry(existingItem).CurrentValues.SetValues(item);
            _context.SaveChanges();

            return item;
        }

        public void Delete(long id) {
            var existingItem = _dataSet.Find(id);

            if (existingItem == null) {
                return;
            }

            _context.Remove(existingItem);
            _context.SaveChanges();
        }

        public bool Exists(long id) {
            return _dataSet.Any(e => e.Id == id);
        }

        public T FindById(long id) {
            return _dataSet.Find(id);
        }

        public List<T> FindAll() {
            return _dataSet.ToList();
        }
    }
}
