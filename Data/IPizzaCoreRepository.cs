using Microsoft.AspNetCore.Http;
using PizzaCore.Data.Entities;
using System.Collections.Generic;
using PizzaCore.Models;
using System.Linq;
using System;

namespace PizzaCore.Data {
  public interface IPizzaCoreRepository {
    IEnumerable<Product> GetAllProducts();
    IEnumerable<ProductByCategory> GetProductsGroupedByCategory();
    Dictionary<string, int> GetDailyProductOrderFrequency();
    Dictionary<string, int> GetWeeklyProductOrderFrequency();
    Dictionary<string, int> GetMonthlyProductOrderFrequency();
    Dictionary<string, int> GetYearlyProductOrderFrequency();
    double GetTotalDailySales();
    double GetTotalWeeklySales();
    double GetTotalMonthlySales();
    double GetTotalYearlySales();
    int GetTotalDailyOrders();
    int GetTotalWeeklyOrders();
    int GetTotalMonthlyOrders();
    int GetTotalYearlyOrders();
    IEnumerable<OrderModel> GetAllOrders();
    ProductSize GetProductSize(int id);
    IEnumerable<CartItem> GetCart(ISession session);
    void AddToCart(ISession session, ProductSize productSize);
    void RemoveFromCart(ISession session, int productId);
    void UpdateCart(ISession session, CartItem cartItem);
    void SaveOrder(OrderModel order, IEnumerable<CartItem> items = null);
    IEnumerable<Product> GetFeaturedProducts();
    void ResetCart(ISession session);
    UserData GetUserDataById(int id);
    UserData GetUserDataByIdentityUserId(string currentUserId);
    bool SaveAll();
  }
}
