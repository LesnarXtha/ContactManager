using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "First Name")]
        [MaxLength(50,ErrorMessage ="Maxixum 50 characters only")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "Maxixum 50 characters only")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Email Address")]
        [MaxLength(100,ErrorMessage = "Maxixum 100 characters only")]
        [EmailAddress]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Phone Number")]
        [MaxLength(15, ErrorMessage = "Maxixum 15 characters only")]
        [Phone]
        public string PhoneNumber { get; set; }

  
        [Display(Name = "Category")]
        [Required(ErrorMessage = "This field is required")]
        public string Category { get; set; } 

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "Tags")]
        [Required(ErrorMessage = "This field is required")]
        public string Tags { get; set; } 

        [Display(Name = "Address")]
        [MaxLength(250, ErrorMessage = "Maxixum 250 characters only")]
        [Required(ErrorMessage = "This field is required")]
        [Column(TypeName = "nvarchar(250)")]

        public string Address { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM-dd-yy}")]
        public DateTime Date { get; set; }
    }
}
