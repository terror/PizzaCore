using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Data.Entities
{
  public class ProductSize
  {
    public int Id { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; }
    public double Price { get; set; }
  }
}
