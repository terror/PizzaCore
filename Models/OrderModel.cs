using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaCore.Models {
  public class OrderModel {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    [Required]
    public string Address { get; set; }

    [Required]
    public string Payment { get; set; }

    public DateTime Date { get; set; }

    public OrderModel setDate(DateTime date) {
      Date = date;
      return this;
    }
  }
}

