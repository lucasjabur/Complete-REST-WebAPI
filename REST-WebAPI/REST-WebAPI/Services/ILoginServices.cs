using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Services {
    public interface ILoginServices {
        TokenDTO? ValidateCredentials(UserDTO dto);
        TokenDTO? ValidateCredentials(TokenDTO token);
        bool RevokeToken(string username);
        AccountCredentialsDTO Create(AccountCredentialsDTO dto);
    }
}
