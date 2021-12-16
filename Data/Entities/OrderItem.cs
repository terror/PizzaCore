using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Data.Entities {
  public class OrderItem {
    public int Id { get; set; }
    public string Size { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public OrderModel Order { get; set; }
  }
}
