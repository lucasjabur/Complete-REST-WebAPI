using REST_WebAPI.Models;
using REST_WebAPI.Models.Context;
using System;

namespace REST_WebAPI.Services.Implementations {
    public class PersonServicesImpl : IPersonServices {

        private MSSQLContext _context;

        public PersonServicesImpl(MSSQLContext context) {
            _context = context;
        }

        public Person Create(Person person) {
            _context.Add(person);
            _context.SaveChanges();

            return person;
        }

        public Person Update(Person person) {
            var existingPerson = _context.People.Find(person.Id);

            if (existingPerson == null) {
                return null;
            }

            _context.Entry(existingPerson).CurrentValues.SetValues(person);
            _context.SaveChanges();

            return person;
        }

        public void Delete(long id) {
            var existingPerson = _context.People.Find(id);

            if (existingPerson == null) {
                return;
            }

            _context.Remove(existingPerson);
            _context.SaveChanges();
        }

        public Person FindById(long id) {
            return _context.People.Find(id);
        }

        public List<Person> FindAll() {

            return _context.People.ToList();
        }
    }
}
