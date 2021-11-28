using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Data.Entities
{

  public class CartItem
  {
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public int CartId { get; set; }
  }
}
