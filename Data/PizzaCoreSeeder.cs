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
        context.AddRange(products);
      }

      // Save changes
      context.SaveChanges();
    }
  }
}
