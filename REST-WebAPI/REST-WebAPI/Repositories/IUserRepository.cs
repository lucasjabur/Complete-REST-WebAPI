using REST_WebAPI.Models;

namespace REST_WebAPI.Repositories {
    public interface IUserRepository : IRepository<User> {
        User? FindByUsername(string username);
    }
}
