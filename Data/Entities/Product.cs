using System.Collections.Generic;

namespace PizzaCore.Data.Entities {
  public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Size { get; set; }
    public double Price { get; set; }
    public string ImageId { get; set; }
  }

  public class ProductByCategory {
    public string Category { get; set; }
    public IEnumerable<Product> Products { get; set; }
  }

  public class ProductByCategoryAndName {
    public string Category { get; set; }
    public string Name { get; set; }
    public IEnumerable<Product> Products { get; set; }
  }
}
