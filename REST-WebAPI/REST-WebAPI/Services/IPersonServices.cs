using REST_WebAPI.Models;

namespace REST_WebAPI.Services {
    public interface IPersonServices {
        Person Create(Person person);
        Person FindById(long id);
        List<Person> FindAll();
        Person Update(Person person);
        void Delete(long id);

    }
}
