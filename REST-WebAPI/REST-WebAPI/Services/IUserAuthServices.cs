using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Models;

namespace REST_WebAPI.Services {
    public interface IUserAuthServices {
        User? FindByUsername(string username);
        User Create(AccountCredentialsDTO dto);
        bool RevokeToken(string username);
        User Update(User user);
    }
}
