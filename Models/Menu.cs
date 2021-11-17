using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Models
{
  public class Menu
  {
    
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public double ItemPrice { get; set; }
    public ItemCategory ItemCategory { get; set; }
    public int ItemCategoryId { get; set; }

  }
}
