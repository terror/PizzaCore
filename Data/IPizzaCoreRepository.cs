using PizzaCore.Data.Entities;
using System.Collections.Generic;

namespace PizzaCore.Data {
  public interface IPizzaCoreRepository {
    IEnumerable<Product> GetAllProducts();
    IEnumerable<ProductByCategory> GetProductsGroupedByCategory();
    bool SaveAll();
  }
}
