using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PizzaCore.Models {
  public class ShowProfileModel {
    [HiddenInput(DisplayValue = false)]
    public int UserDataId { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public List<IdentityRole> Roles { get; set; }
    public List<OrderModel> Orders { get; set; }
  }
}
