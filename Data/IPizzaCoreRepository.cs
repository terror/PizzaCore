using Microsoft.AspNetCore.Http;
using PizzaCore.Data.Entities;
using System.Collections.Generic;
using PizzaCore.Models;

namespace PizzaCore.Data {
  public interface IPizzaCoreRepository {
    IEnumerable<Product> GetAllProducts();
    IEnumerable<ProductByCategory> GetProductsGroupedByCategory();
    ProductSize GetProductSize(int id);
    IEnumerable<CartItem> GetCart(ISession session);
    void AddToCart(ISession session, ProductSize productSize);
    void RemoveFromCart(ISession session, int productId);
    void SaveOrder(OrderModel order);
    IEnumerable<Product> GetFeaturedProducts();
    bool SaveAll();
  }
}
