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
        // The order we want
        var order = new List<string> { "Pizzas", "Burgers", "Fries", "Drinks" };
        // Category -> [Product]
        return GetAllProducts()
          .GroupBy(p => p.Category)
          .Select(group => new ProductByCategory { Category = group.Key, Products = group.ToList() })
          .OrderBy(group => order.IndexOf(group.Category))
          .ToList();
      } catch (Exception ex) {
        logger.LogError($"Failed to group products by category: {ex.Message}");
        return null;
      }
    }

    public IEnumerable<ProductByCategoryAndName> GetProductsGroupedByCategoryAndName() {
      try {
        logger.LogInformation("[PizzaRepository::GetProductsGroupedByCategoryAndName] Grouping products by category and name ...");
        // The order we want
        var order = new List<string> { "Pizzas", "Burgers", "Fries", "Drinks" };
        // Category -> [Product]
        return GetAllProducts()
          .GroupBy(p => new { p.Category, p.Name })
          .Select(group => new ProductByCategoryAndName { Category = group.Key.Category, Name = group.Key.Name, Products = group.ToList() })
          .OrderBy(group => order.IndexOf(group.Category))
          .ToList();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to group products by category and title: {ex.Message}");
        return null;
      }
    }

    public bool SaveAll() {
      return context.SaveChanges() > 0;
    }
  }
}
