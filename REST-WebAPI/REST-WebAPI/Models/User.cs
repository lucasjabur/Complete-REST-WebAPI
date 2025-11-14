using REST_WebAPI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_WebAPI.Models {

    [Table("users")]
    public class User : BaseEntity {
        [Column("user_name")]
        public string Username { get; set; } = string.Empty;

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("refresh_token")]
        public string? RefreshToken { get; set; } = string.Empty;

        [Column("refresh_token_expiry_time")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }
}
