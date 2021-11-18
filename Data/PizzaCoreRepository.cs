using Microsoft.Extensions.Logging;
using PizzaCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaCore.Data {
  public class PizzaCoreRepository : IPizzaCoreRepository {
    private readonly PizzaCoreContext context;
    private readonly ILogger<PizzaCoreRepository> logger;

    public PizzaCoreRepository(PizzaCoreContext context, ILogger<PizzaCoreRepository> logger) {
      this.context = context;
      this.logger = logger;
    }

    public IEnumerable<Product> GetAllProducts() {
      try {
        logger.LogInformation("[PizzaRepository::GetAllProducts] Getting all products ...");
        return context.Products;
      } catch (Exception ex) {
        logger.LogError($"Failed to get all products: {ex.Message}");
        return null;
      }
    }

    public IEnumerable<ProductByCategory> GetProductsGroupedByCategory() {
      try {
        logger.LogInformation("[PizzaRepository::GetProductsGroupedByCategory] Grouping products by category ...");
        // Grab all products
        var products = GetAllProducts();
        // Category -> [Product]
        return products
          .GroupBy(p => p.Category)
          .Select(group => new ProductByCategory { Category = group.Key, Products = group.ToList() })
          .ToList();
      } catch (Exception ex) {
        logger.LogError($"Failed to group products by category: {ex.Message}");
        return null;
      }
    }

    public bool SaveAll() {
      return context.SaveChanges() > 0;
    }
  }
}
