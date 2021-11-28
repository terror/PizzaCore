using Microsoft.AspNetCore.Http;
using PizzaCore.Data.Entities;
using System.Collections.Generic;

namespace PizzaCore.Data {
  public interface IPizzaCoreRepository {
    IEnumerable<Product> GetAllProducts();
    IEnumerable<ProductByCategory> GetProductsGroupedByCategory();
    IEnumerable<CartItem> GetCart(ISession session);
    void AddToCart(ISession session, int productId);
    void RemoveFromCart(ISession session, int productId);
    bool SaveAll();
  }
}
