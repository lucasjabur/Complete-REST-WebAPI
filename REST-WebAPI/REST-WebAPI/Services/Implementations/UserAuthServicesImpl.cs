using REST_WebAPI.Auth.Contract;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Models;
using REST_WebAPI.Repositories;

namespace REST_WebAPI.Services.Implementations {
    public class UserAuthServicesImpl : IUserAuthServices {

        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UserAuthServicesImpl(IUserRepository repository, IPasswordHasher passwordHasher) {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public User Create(AccountCredentialsDTO dto) {
            if (dto == null) {
                throw new ArgumentNullException(nameof(dto));
            }

            var entity = new User {
                Username = dto.Username,
                FullName = dto.Fullname,
                Password = _passwordHasher.Hash(dto.Password),
                RefreshToken = string.Empty,
                RefreshTokenExpiryTime = null
            };

            return _repository.Create(entity);
        }
        public User Update(User user) {
            return _repository.Update(user);
        }

        public User? FindByUsername(string username) {
            return _repository.FindByUsername(username);
        }

        public bool RevokeToken(string username) {
            var user = _repository.FindByUsername(username);
            if (user == null) {
                return false;
            }

            user.RefreshToken = null;
            _repository.Update(user);

            return true;
        }

    }
}
