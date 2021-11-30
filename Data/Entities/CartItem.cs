using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Data.Entities
{

  public class CartItem
  {
    public ProductSize ProductSize { get; set; }
    public int Quantity { get; set; }
  }
}
