using PizzaCore.Data.Entities;
using System;
using System.Collections.Generic;
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

    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }

    [Required]
    public string Method { get; set; }

    public string Address { get; set; }

    [RegularExpression("^[A-Za-z-.' ]+$", ErrorMessage = "The City field is not a valid city.")]
    [MinLength(2, ErrorMessage = "The City field is not a valid city.")]
    [MaxLength(50, ErrorMessage = "The City field is not a valid city.")]
    public string City { get; set; }

    [RegularExpression("^[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ ]\\d[ABCEGHJ-NPRSTV-Z]\\d$", ErrorMessage = "Error! Must be a valid postal code (X1X 1X1).")]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; }

    [Required]
    public string Payment { get; set; }

    public DateTime Date { get; set; }

    public double SubTotal { get; set; }

    public double ShippingCost { get; set; }

    public double Taxes { get; set; }

    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    public OrderModel setDate(DateTime date) {
      Date = date;
      return this;
    }

    public double GetTotal() {
      return SubTotal + ShippingCost + Taxes;
    }

    public const int DECIMAL_NUM = 2;
    public const double GST_TAX_RATE = 0.05;
    public const double QST_TAX_RATE = 0.09975;

    public static double[] generateSummary(double cartTotal) {
      double subTotal = Math.Round(cartTotal, DECIMAL_NUM);
      double gstTax = Math.Round(subTotal * GST_TAX_RATE, DECIMAL_NUM);
      double qstTax = Math.Round(subTotal * QST_TAX_RATE, DECIMAL_NUM);
      double finalPrice = Math.Round(subTotal + gstTax + qstTax, DECIMAL_NUM);
      return new double[] { subTotal, gstTax, qstTax, finalPrice };
    }
  }
}

