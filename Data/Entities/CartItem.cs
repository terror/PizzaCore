using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Data.Entities
{
  public class CartItem
  {
    public int Id { get; set; }
    public Product Product { get; set; }
    public int ProductId { get; set; }

  }
}
