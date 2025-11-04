using REST_WebAPI.Models;
using REST_WebAPI.Repositories;
using REST_WebAPI.Repositories.Implementations;

namespace REST_WebAPI.Services.Implementations {
    public class PersonServicesImpl : IPersonServices {

        private IPersonRepository _repository;

        public PersonServicesImpl(IPersonRepository repository) {
            _repository = repository;
        }

        public Person Create(Person person) {
            return _repository.Create(person);
        }

        public Person Update(Person person) {
            return _repository.Update(person);
        }

        public void Delete(long id) {
            _repository.Delete(id);
        }

        public Person FindById(long id) {
            return _repository.FindById(id);
        }

        public List<Person> FindAll() {
            return _repository.FindAll();
        }
    }
}
