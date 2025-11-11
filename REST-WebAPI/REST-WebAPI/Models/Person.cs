using REST_WebAPI.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_WebAPI.Models {
    [Table("person")]
    public class Person : BaseEntity {

        [Required]
        [Column("first_name", TypeName = "varchar(80)")]
        [MaxLength(80)]
        public string FirstName { get; set; }

        [Required]
        [Column("last_name", TypeName = "varchar(80)")]
        [MaxLength(80)]
        public string LastName { get; set; }

        [Required]
        [Column("address", TypeName = "varchar(100)")]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [Column("gender", TypeName = "varchar(6)")]
        [MaxLength(6)]
        public string Gender { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }

        // [NotMapped]
        // [Column("birth_day")]
        // public DateTime? BirthDay { get; set; }
    }
}
