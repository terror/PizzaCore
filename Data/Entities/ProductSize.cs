using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaCore.Data.Entities {
  public class ProductSize {
    public int Id { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public string Size { get; set; }
    public double Price { get; set; }
  }
}
