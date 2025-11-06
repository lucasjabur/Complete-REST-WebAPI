using REST_WebAPI.Data.Converter.Implementations;
using REST_WebAPI.Data.DTO.V2;
using REST_WebAPI.Models;
using REST_WebAPI.Repositories;

namespace REST_WebAPI.Services.Implementations {
    public class PersonServicesImplV2 {

        private IRepository<Person> _repository;
        private readonly PersonConverter _converter;

        public PersonServicesImplV2(IRepository<Person> repository) {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public PersonDTO Create(PersonDTO person) {
            var entity = _converter.Parse(person);
            entity = _repository.Create(entity);
            return _converter.Parse(entity);
        }
    }
}
