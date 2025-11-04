using REST_WebAPI.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_WebAPI.Models {

    [Table("books")]
    public class Book : BaseEntity {

        [Column("title")]
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }

        [Column("author")]
        [Required(ErrorMessage = "Author is required!")]
        public string Author { get; set; }

        [Column("price")]
        [Required(ErrorMessage = "Price is required!")]
        public decimal Price { get; set; }

        [Column("launch_date")]
        [Required(ErrorMessage = "Launch date is required!")]
        public DateTime LaunchDate { get; set; }
    }
}
