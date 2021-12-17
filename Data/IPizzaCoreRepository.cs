using Microsoft.AspNetCore.Http;
using PizzaCore.Data.Entities;
using System.Collections.Generic;
using PizzaCore.Models;
using System.Linq;
using System;

namespace PizzaCore.Data {
  public interface IPizzaCoreRepository {
    IEnumerable<Product> GetAllProducts();
    Product GetProduct(int id);
    IEnumerable<ProductByCategory> GetProductsGroupedByCategory();
    void UpdateOrderStatus(int orderId, Status status);
    void DeleteOrder(int orderId);
    IEnumerable<OrderModel> GetAllOrders();
    OrderModel GetOrderById(int orderId);
    IEnumerable<OrderModel> GetTodayOrders();
    IEnumerable<OrderModel> GetYesterdaysOrders();
    IEnumerable<OrderModel> GetLastWeekOrders();
    IEnumerable<OrderModel> GetLastMonthOrders();
    IEnumerable<OrderModel> GetLastYearOrders();
    double GetTotalDailySales();
    double GetTotalWeeklySales();
    double GetTotalMonthlySales();
    double GetTotalYearlySales();
    int GetTotalDailyOrders();
    int GetTotalWeeklyOrder();
    int GetTotalMonthlyOrders();
    int GetTotalYearlyOrders();
    Dictionary<string, int> GetDailyProductOrderFrequency();
    Dictionary<string, int> GetWeeklyProductOrderFrequency();
    Dictionary<string, int> GetMonthlyProductOrderFrequency();
    Dictionary<string, int> GetYearlyProductOrderFrequency();
    ProductSize GetProductSize(int id);
    IEnumerable<CartItem> GetCart(ISession session);
    void AddToCart(ISession session, ProductSize productSize);
    void RemoveFromCart(ISession session, int productId);
    void UpdateCart(ISession session, CartItem cartItem);
    int SaveOrder(OrderModel order, IEnumerable<CartItem> items = null);
    void UpdateOrder(OrderModel order);
    IEnumerable<Product> GetFeaturedProducts();
    void ResetCart(ISession session);
    UserData GetUserDataById(int id);
    UserData GetUserDataByIdentityUserId(string currentUserId);
    void DeleteUserDataByIdentityUserId(string id);
    bool SaveAll();
  }
}
