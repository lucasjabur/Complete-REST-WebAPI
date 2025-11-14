namespace REST_WebAPI.Auth.Contract {
    public interface IPasswordHasher {
        string Hash(string password);
        bool Verify(string password, string hashedPassword);
    }
}
