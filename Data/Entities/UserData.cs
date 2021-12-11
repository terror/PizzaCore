using PizzaCore.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaCore.Data.Entities {
  public class UserData {
    [Key]
    public int UserDataId { get; set; }
    public string IdentityUserId { get; set; }
    public string Address { get; set; }
    public List<OrderModel> Orders { get; set; }
  }
}
