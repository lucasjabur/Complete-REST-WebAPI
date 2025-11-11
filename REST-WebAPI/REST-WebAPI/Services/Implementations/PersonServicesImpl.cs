using Mapster;
using REST_WebAPI.Models;
using REST_WebAPI.Repositories;
using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Services.Implementations {
    public class PersonServicesImpl : IPersonServices {

        private IPersonRepository _repository;
        public PersonServicesImpl(IPersonRepository repository) {
            _repository = repository;
        }

        public List<PersonDTO> FindAll() {
            return _repository.FindAll().Adapt<List<PersonDTO>>();
        }

        public PersonDTO FindById(long id) {
            return _repository.FindById(id).Adapt<PersonDTO>();
        }

        public PersonDTO Create(PersonDTO person) {
            var entity = person.Adapt<Person>();
            entity = _repository.Create(entity);
            return entity.Adapt<PersonDTO>();
        }

        public PersonDTO Update(PersonDTO person) {
            var entity = person.Adapt<Person>();
            entity = _repository.Update(entity);
            return entity.Adapt<PersonDTO>();
        }
        public void Delete(long id) {
            _repository.Delete(id);
        }

        public PersonDTO Disable(long id) {
            var entity = _repository.Disable(id);
            return entity.Adapt<PersonDTO>();
        }
    }
}
