using Microsoft.AspNetCore.Identity;
using REST_WebAPI.Auth.Contract;
using System.Security.Cryptography;
using System.Text;

namespace REST_WebAPI.Auth.Tools {
    public class Sha256PasswordHasher : IPasswordHasher {
        public string Hash(string password) {
            var inputBytes = Encoding.UTF8.GetBytes(password);
            var hashedBytes = SHA256.HashData(inputBytes);

            var builder = new StringBuilder();
            foreach (var bytes in hashedBytes) {
                builder.Append(bytes.ToString("x2")); // number to hex
            }

            return builder.ToString();
        }

        public bool Verify(string password, string hashedPassword) {
            return Hash(password) == hashedPassword;
        }
    }
}
