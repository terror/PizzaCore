using System.ComponentModel.DataAnnotations;

namespace PizzaCore.Models
{
  public class DeliveryInfoModel {
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "The field First Name must be a string with a minimum length of 2 and a maximum length of 50.")]
    [MaxLength(50, ErrorMessage = "The field First Name must be a string with a minimum length of 2 and a maximum length of 50.")]
    [RegularExpression("^[^0-9]+$", ErrorMessage = "First name cannot contain numbers.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "The field Last Name must be a string with a minimum length of 2 and a maximum length of 50.")]
    [MaxLength(50, ErrorMessage = "The field Last Name must be a string with a minimum length of 2 and a maximum length of 50.")]
    [RegularExpression("^[^0-9]+$", ErrorMessage = "Last name cannot contain numbers.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [RegularExpression("^[\\.a-zA-Z0-9]+@[^@\\s]+\\.[^@\\s]+$", ErrorMessage = "The Email field is not a valid e-mail address.")]
    [MinLength(2, ErrorMessage = "The Email field is not a valid e-mail address.")]
    [MaxLength(30, ErrorMessage = "The Email field is not a valid e-mail address.")]
    public string Email { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }

    [Required]
    [RegularExpression("^[A-Za-z0-9-.' ]+$", ErrorMessage = "The Address field is not a valid address.")]
    [MinLength(4, ErrorMessage = "The Address field is not a valid address.")]
    [MaxLength(50, ErrorMessage = "The Address field is not a valid address.")]
    public string Address { get; set; }

    [Required]
    [RegularExpression("^[A-Za-z-.' ]+$", ErrorMessage = "The City field is not a valid city.")]
    [MinLength(2, ErrorMessage = "The City field is not a valid city.")]
    [MaxLength(50, ErrorMessage = "The City field is not a valid city.")]
    public string City { get; set; }

    [Required]
    [RegularExpression("^[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ ]\\d[ABCEGHJ-NPRSTV-Z]\\d$", ErrorMessage = "Error! Must be a valid postal code (X1X 1X1).")]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; }
  }
}
