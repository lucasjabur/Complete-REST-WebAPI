using REST_WebAPI.Data.Converter.Contract;
using REST_WebAPI.Data.DTO.V2;
using REST_WebAPI.Models;

namespace REST_WebAPI.Data.Converter.Implementations {
    public class PersonConverter : IParser<Person, PersonDTO>, IParser<PersonDTO, Person> {
        public Person Parse(PersonDTO origin) {
            if (origin == null) return null;
            return new Person {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender,
                // BirthDay = origin.BirthDay
            };
        }

        public PersonDTO Parse(Person origin) {
            if (origin == null) return null;
            return new PersonDTO {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender,
                // BirthDay = DateTime.Now
                // BirthDay = origin.BirthDay ?? DateTime.Now
            };
        }

        public List<Person> ParseList(List<PersonDTO> origin) {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<PersonDTO> ParseList(List<Person> origin) {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}
