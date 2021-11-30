using Microsoft.AspNetCore.Hosting;
using PizzaCore.Data.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PizzaCore.Data {
  public class PizzaCoreSeeder {
    private readonly PizzaCoreContext context;
    private readonly IWebHostEnvironment env;

    // Path to our sample product data
    private const string SAMPLE_DATA_FILEPATH = "Data/Seeds/products.json";

    public PizzaCoreSeeder(PizzaCoreContext context, IWebHostEnvironment env) {
      this.context = context;
      this.env = env;
    }

    public void Seed() {
      // Verify that the database actually exists
      context.Database.EnsureCreated();

      // If there are no menu items, create sample data
      if (!context.Products.Any()) {
        // Add a range of products deserialized from our sample json data
        var products = JsonSerializer.Deserialize<IEnumerable<Product>>(File.ReadAllText(Path.Combine(env.ContentRootPath, SAMPLE_DATA_FILEPATH)));
        SeedSizes(products);
        context.AddRange(products);
      }

      // Save changes
      context.SaveChanges();
    }

    private void SeedSizes(IEnumerable<Product> products) {
      // Seed the appropriate product sizes depending on the products category.
      foreach(var product in products) {
        if (product.Category == "Pizzas") {
          product.ProductSizes = new List<ProductSize> {
            new ProductSize() { Product = product, Size = "Small", Price = 10.99 },
            new ProductSize() { Product = product, Size = "Medium", Price = 13.99 },
            new ProductSize() { Product = product, Size = "Large", Price = 15.99 }
          };
        }
        else if (product.Category == "Drinks") {
          product.ProductSizes = new List<ProductSize> {
            new ProductSize() { Product = product, Size = "OS", Price = 2.99 }
          };
        }
        else if (product.Category == "Burgers") {
          product.ProductSizes = new List<ProductSize> {
            new ProductSize() { Product = product, Size = "OS", Price = 11.99 }
          };
        }
        else if (product.Category == "Fries") {
          product.ProductSizes = new List<ProductSize> {
            new ProductSize() { Product = product, Size = "Small", Price = 2.99 },
            new ProductSize() { Product = product, Size = "Medium", Price = 4.99 },
            new ProductSize() { Product = product, Size = "Large", Price = 5.99 }
          };
        }
      }
    }
  }
}
