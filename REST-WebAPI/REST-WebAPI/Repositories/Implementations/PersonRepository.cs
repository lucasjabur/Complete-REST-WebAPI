using Mapster;
using REST_WebAPI.Hypermedia.Utils;
using REST_WebAPI.Models;
using REST_WebAPI.Models.Context;
using REST_WebAPI.Repositories.QueryBuilders;

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

        public List<Person> FindByName(string firstName, string lastName) {
            var query = _context.People.AsQueryable();
            if (!string.IsNullOrWhiteSpace(firstName)) {
                query = query.Where(p => p.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrWhiteSpace(lastName)) {
                query = query.Where(p => p.LastName.Contains(lastName));
            }

            // return query.ToList();
            return [.. query];
        }

        public PagedSearch<Person> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page) {
            var queryBuilder = new PersonQueryBuilder();
            var (query, countQuery, sort, size, offset) = queryBuilder.BuildQueries(name, sortDirection, pageSize, page);

            var people = base.FindWithPagedSearch(query);
            var totalResults = base.GetCount(countQuery);

            return new PagedSearch<Person> {
                CurrentPage = page,
                List = people,
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }
    }
}
