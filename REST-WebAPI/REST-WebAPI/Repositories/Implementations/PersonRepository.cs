using REST_WebAPI.Models;
using REST_WebAPI.Models.Context;

namespace REST_WebAPI.Repositories.Implementations {
    public class PersonRepository(MSSQLContext context)
        : GenericRepository<Person>(context), IPersonRepository {
        public Person Disable(long id) {
            var person = _context.People.Find(id);
            if (person == null) return null;
            person.Enabled = false;
            _context.SaveChanges();
            return person;
        }
    }
}
