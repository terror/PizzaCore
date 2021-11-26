using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PizzaCore.Data.Entities {
  public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public IEnumerable<ProductSize> Sizes { get; set; }
    public string ImageId { get; set; }

    /// <summary>
    /// Creates a JSON string consisting of the product sizes and their corresponding prices.
    /// Used when updating the price after a user makes a size selection from the menu view.
    /// </summary>
    /// <returns></returns>
    public string GetPricesBySizeAsJson() {
      return JsonSerializer.Serialize(Sizes.Select(s => new { Size = s.Size, Price = s.Price}));
    }
  }

  public class ProductSize {
    public int Id { get; set; }
    public Product Product { get; set; }
    public string Size { get; set; }
    public double Price { get; set; }
  }

  public class ProductByCategory {
    public string Category { get; set; }
    public IEnumerable<Product> Products { get; set; }
  }
}
