using REST_WebAPI.Models;
using REST_WebAPI.Models.Base;

namespace REST_WebAPI.Repositories {
    public interface IRepository<T> where T : BaseEntity {
        
        T Create(T item);
        T Update(T item);
        void Delete(long id);
        bool Exists(long id);
        T FindById(long id);
        List<T> FindAll();
    }
}
