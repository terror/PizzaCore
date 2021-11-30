using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PizzaCore.Data.Entities {
  public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public List<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    public string ImageId { get; set; }

    /*
     * Creates a JSON string consisting of the product sizes and their corresponding prices.
     * Used when updating the price after a user makes a size selection from the menu view.
     */
    public string GetPricesBySizeAsJson() {
      return JsonSerializer.Serialize(ProductSizes.Select(s => new { Id = s.Id, Size = s.Size, Price = s.Price}));
    }
  }

  public class ProductByCategory {
    public string Category { get; set; }
    public IEnumerable<Product> Products { get; set; }
  }
}
