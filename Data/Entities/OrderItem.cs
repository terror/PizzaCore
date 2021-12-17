using PizzaCore.Models;
using System;

namespace PizzaCore.Data.Entities {
  public class OrderItem {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Size { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public OrderModel Order { get; set; }
    public Status Status { get; set; }
    public DateTime PreparingTimeStamp { get; set; }
    public DateTime ReadyTimeStamp { get; set; }
  }
}

