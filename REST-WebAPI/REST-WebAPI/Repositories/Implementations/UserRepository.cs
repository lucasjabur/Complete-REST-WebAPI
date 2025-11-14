using REST_WebAPI.Models;
using REST_WebAPI.Models.Context;

namespace REST_WebAPI.Repositories.Implementations {
    public class UserRepository : GenericRepository<User>, IUserRepository {
        public UserRepository(MSSQLContext context) : base(context) { }

        public User FindByUsername(string username) {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }
    }
}
