using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_WebAPI.Models {

    [Table("person")]
    public class Person {

        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(80)]
        [Column("first_name", TypeName = "varchar(80)")]
        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; }

        [MaxLength(80)]
        [Column("last_name", TypeName = "varchar(80)")]
        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }

        [MaxLength(100)]
        [Column("address", TypeName = "varchar(100)")]
        [Required(ErrorMessage = "Address is required!")]
        public string Address { get; set; }

        [MaxLength(6)]
        [Column("gender", TypeName = "varchar(6)")]
        [Required(ErrorMessage = "Address is required!")]
        public string Gender { get; set; }
    }
}
