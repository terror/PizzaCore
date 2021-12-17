using System.ComponentModel.DataAnnotations;

namespace PizzaCore.Models {
  public class ManagementModel {
    public int UserId { get; set; }

    [DataType(DataType.EmailAddress)]
    [RegularExpression("^[\\.a-zA-Z0-9]+@[^@\\s]+\\.[^@\\s]+$", ErrorMessage = "The Email field is not a valid e-mail address.")]
    [MinLength(2, ErrorMessage = "The Email field is not a valid e-mail address.")]
    [MaxLength(30, ErrorMessage = "The Email field is not a valid e-mail address.")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string Role { get; set; }

    public string Message { get; set; }
  }
}
