using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaCore.Models {
  public class ShowProfileModel {
    [HiddenInput(DisplayValue = false)]
    public int UserDataId { get; set; }

    [MinLength(2, ErrorMessage = "The field First Name must be a string with a minimum length of 2 and a maximum length of 50.")]
    [MaxLength(50, ErrorMessage = "The field First Name must be a string with a minimum length of 2 and a maximum length of 50.")]
    [RegularExpression("^[^0-9]+$", ErrorMessage = "First name cannot contain numbers.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [MinLength(2, ErrorMessage = "The field Last Name must be a string with a minimum length of 2 and a maximum length of 50.")]
    [MaxLength(50, ErrorMessage = "The field Last Name must be a string with a minimum length of 2 and a maximum length of 50.")]
    [RegularExpression("^[^0-9]+$", ErrorMessage = "Last name cannot contain numbers.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    [RegularExpression("^[\\.a-zA-Z0-9]+@[^@\\s]+\\.[^@\\s]+$", ErrorMessage = "The Email field is not a valid e-mail address.")]
    [MinLength(2, ErrorMessage = "The Email field is not a valid e-mail address.")]
    [MaxLength(30, ErrorMessage = "The Email field is not a valid e-mail address.")]
    public string Email { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    [RegularExpression(@"[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]", ErrorMessage = "Error! Must be a valid postal code (X1X 1X1).")]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; }

    public List<IdentityRole> Roles { get; set; }

    public List<OrderModel> Orders { get; set; }
  }
}
