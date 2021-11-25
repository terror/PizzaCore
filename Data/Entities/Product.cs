using System.Collections.Generic;

namespace PizzaCore.Data.Entities {
  public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public double Price { get; set; }
    public string ImageId { get; set; }
    public bool IsFeatured { get; set; }
  }

  public class ProductByCategory {
    public string Category { get; set; }
    public IEnumerable<Product> Products { get; set; }
  }
}
