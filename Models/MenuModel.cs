using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Models
{
  public class MenuModel
  {
    public enum Category
    {
      Pizzas,
      Drinks,
      Burgers,
      Fries
    };
    
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public double ItemPrice { get; set; }
    public string ItemCategory { get; set; }
    public byte[] ItemImage { get; set; }


  }
}
