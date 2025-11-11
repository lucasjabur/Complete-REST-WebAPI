using REST_WebAPI.Models;

namespace REST_WebAPI.Repositories {
    public interface IPersonRepository : IRepository<Person> {
        Person Disable(long id);
    }
}
