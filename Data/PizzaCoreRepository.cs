using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PizzaCore.Data.Entities;
using PizzaCore.Helpers;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace PizzaCore.Data {
  public class PizzaCoreRepository : IPizzaCoreRepository {
    private readonly PizzaCoreContext context;
    private readonly ILogger<PizzaCoreRepository> logger;
    private const string SESSION_KEY_CART = "cart";

    public PizzaCoreRepository(PizzaCoreContext context, ILogger<PizzaCoreRepository> logger) {
      this.context = context;
      this.logger = logger;
    }

    public IEnumerable<Product> GetAllProducts() {
      try {
        logger.LogInformation("[PizzaRepository::GetAllProducts] Getting all products ...");

        var products = context.Products.ToList();

        // Get all the sizes for each product
        foreach (var product in products) {
          product.ProductSizes = context.ProductSizes
            .Where(ps => ps.ProductId.Equals(product.Id))
            .OrderBy(ps => ps.Price)
            .ToList();
        }

        return products;
      } catch (Exception ex) {
        logger.LogError($"Failed to get all products: {ex.Message}");
        return null;
      }
    }

    private Product GetProduct(int id) {
        return context.Products.Find(id);
    }

    public IEnumerable<Product> GetFeaturedProducts()
    {
      try
      {
        logger.LogInformation("[PizzaRepository::GetFeaturedProducts] Getting featured products ...");
        return GetAllProducts()
          .Where(p => p.IsFeatured)
          .ToList();
    }
      catch (Exception ex)
      {
        logger.LogError($"Failed to get featured products: {ex.Message}");
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

    public ProductSize GetProductSize(int id) {
      try {
        ProductSize productSize = context.ProductSizes.Find(id);
        productSize.Product = GetProduct(productSize.ProductId);
        return productSize;
      } catch (Exception ex) {
        logger.LogError($"Failed to get product size of id {id} : {ex.Message}");
        return null;
      }
    }

    public IEnumerable<CartItem> GetCart(ISession session) {
      // get cart from session (null if cart does not exist)
      return session.GetObjectFromJson<List<CartItem>>(SESSION_KEY_CART);
    }

    public void AddToCart(ISession session, ProductSize productToAdd) {
      try {
        logger.LogInformation("[PizzaRepository::AddToCart] Adding product to cart...");

        // Validate product
        if (productToAdd == null)
          throw new ArgumentNullException("productToAdd", $"Given ProductSize cannot be null");

        // Get the cart
        var cartList = GetCart(session);

        // Convert IEnumerable cartList to List<CartItem> so we can add to it.
        List<CartItem> cart;
        if (cartList != null)
          cart = cartList.ToList();
        else
          cart = new List<CartItem>();


        // Check if this product already exists in the cart
        int productCartIndex = FindProductInCart(cart, productToAdd.Id);

        if (productCartIndex != -1) {
          cart[productCartIndex].Quantity++;    // Increment quantity because same
                                                // product with this size already exists in cart
        } else {
          cart.Add(new CartItem {
            ProductSize = productToAdd,
            Quantity = 1    // quantity is 1 because no same product with this size is in cart
          });
        }

        // Set the new cart with added product in the session.
        session.SetObjectAsJson(SESSION_KEY_CART, cart);
      } catch (Exception ex) {
        logger.LogError($"Failed to add product to cart : {ex.Message}");
      }

    }

    public void RemoveFromCart(ISession session, int productSizeId) {
      try {
        logger.LogInformation("[PizzaRepository::RemoveFromCart] Removing product from cart");

        // Get cart and remove product
        List<CartItem> cart = GetCart(session).ToList();
        int productCartIndex = FindProductInCart(cart, productSizeId);
        cart.RemoveAt(productCartIndex);

        // Set the updated cart in the session
        session.SetObjectAsJson(SESSION_KEY_CART, cart);

      } catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is NullReferenceException) {
        logger.LogError($"Failed to remove product of id {productSizeId} from cart : Product of id {productSizeId} does not exist in the cart");
      } catch (Exception ex) {
        logger.LogError($"Failed to remove product from cart: {ex.Message}");
      }
    }

    public void UpdateCart(ISession session, CartItem cartItem)
    {
      try
      {
        logger.LogInformation("[PizzaRepository::UpdateCart] Updating cart item in cart");

        // Validate cartItem state
        if(cartItem.Quantity <= 0) {
          throw new ArgumentException("cannot update cart item to have quantity of 0 or less");
        }

        // Get cart from session
        List<CartItem> cart = GetCart(session).ToList();

        // Updating the cartItem and set the updated cart in the session
        int productCartIndex = FindProductInCart(cart, cartItem.ProductSize.Id);
        cart[productCartIndex] = cartItem;
        session.SetObjectAsJson(SESSION_KEY_CART, cart);

      }
      catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is NullReferenceException) {
        logger.LogError($"Failed to update product of id {cartItem.ProductSize.Id} in cart : Product of id {cartItem.ProductSize.Id} does not exist in the cart");
      }
      catch (ArgumentException ex)
      {
        logger.LogError($"Failed to update product in cart : Given {ex.ParamName} is not in a valid state");
      }
      catch (Exception ex) {
        logger.LogError($"Failed to update product in cart: {ex.Message}");
      }
    }

    private int FindProductInCart(List<CartItem> cart, int productSizeId) {
      return cart.FindIndex(item => item.ProductSize.Id == productSizeId);
    }

    public void SaveOrder(OrderModel order, IEnumerable<CartItem> items = null) {
      try {
        logger.LogInformation("[PizzaCoreRepository::SaveOrder] Saving order...");

        // If items is not null, add each cart item as an order item in the order
        if (items != null) {
          foreach (CartItem item in items) {
            order.Items.Add(new OrderItem() {
              Size = item.ProductSize.Size,
              Price = item.ProductSize.Price,
              Quantity = item.Quantity,
              ProductId = item.ProductSize.Product.Id
            });
          }
        }

        context.Add(order);
        SaveAll();
      } catch(Exception ex) {
        logger.LogError($"Failed to save order: {ex.Message}");
      }
    }

    public UserData GetUserDataById(int id) {
      return context.UserDatas.SingleOrDefault(p => p.UserDataId == id);
    }

    public UserData GetUserDataByIdentityUserId(string currentUserId) {
      return context.UserDatas.SingleOrDefault(p => p.IdentityUserId == currentUserId);
    }

    public void SetUserDataByUserId(string currentUserId, UserData userData) {
      try {
        var oldUserData = GetUserDataByIdentityUserId(currentUserId);
        if (oldUserData == null) {
          context.UserDatas.Add(userData);
        } else {
          oldUserData.FirstName = userData.FirstName ?? oldUserData.FirstName;
          oldUserData.LastName = userData.LastName ?? oldUserData.LastName;
          oldUserData.Address = userData.Address ?? oldUserData.Address;
          oldUserData.City = userData.City ?? oldUserData.City;
          oldUserData.PostalCode = userData.PostalCode ?? oldUserData.FirstName;
        }
        SaveAll();
      } catch (Exception ex) {
        logger.LogError($"Failed to set user data: {ex.Message}");
      }
    }

    public void DeleteUserDataByIdentityUserId(string id) {
      try {
        logger.LogInformation("[PizzaCoreRepository::DeleteUserDataByIdentityUserId] Removing UserData...");
        var data = context.UserDatas.FirstOrDefault(p => p.IdentityUserId == id);
        context.UserDatas.Remove(data);
        SaveAll();
      } catch (Exception ex) {
        logger.LogError($"Failed to remove user data: {ex.Message}");
      }
    }

    public bool SaveAll() {
      return context.SaveChanges() > 0;
    }

    public void ResetCart(ISession session) {
      session.Remove(SESSION_KEY_CART);
    }

    public IEnumerable<OrderModel> GetAllOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetAllOrders] Getting all orders...");

        // Get all orders and order items
        var orders = context.OrderModels.ToList();
        var orderItems = context.OrderItems.ToList();

        // Add the order items to their respective order
        foreach (var order in orders) {
          order.Items = orderItems.Where(i => i.Order.Id == order.Id).ToList();
        }

        return orders;
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get all orders: {ex.Message}");
        return null;
      }
    }

    public Dictionary<string, int> GetDailyProductOrderFrequency() {
      try {
        logger.LogInformation("[PizzaRepository::GetDailyProductOrderFrequency] Getting daily product order frequency...");
        return GetProductOrderFrequency(GetTodayOrders().Select(o => o.Items).ToList());
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get daily product order frequency: {ex.Message}");
        return null;
      }
    }

    public Dictionary<string, int> GetWeeklyProductOrderFrequency() {
      try {
        logger.LogInformation("[PizzaRepository::GetWeeklyProductOrderFrequency] Getting weekly product order frequency...");
        return GetProductOrderFrequency(GetLastWeekOrders().Select(o => o.Items).ToList());
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get weekly product order frequency: {ex.Message}");
        return null;
      }
    }

    public Dictionary<string, int> GetMonthlyProductOrderFrequency() {
      try {
        logger.LogInformation("[PizzaRepository::GetMonthlyProductOrderFrequency] Getting monthly product order frequency...");
        return GetProductOrderFrequency(GetLastMonthOrders().Select(o => o.Items).ToList());
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get monthly product order frequency: {ex.Message}");
        return null;
      }
    }

    public Dictionary<string, int> GetYearlyProductOrderFrequency() {
      try {
        logger.LogInformation("[PizzaRepository::GetYearlyProductOrderFrequency] Getting yearly product order frequency...");
        return GetProductOrderFrequency(GetLastYearOrders().Select(o => o.Items).ToList());
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get yearly product order frequency: {ex.Message}");
        return null;
      }
    }

    private Dictionary<string, int> GetProductOrderFrequency(IEnumerable<IEnumerable<OrderItem>> itemsbyOrder) {
      Dictionary<string, int> productOrderFrequency = new Dictionary<string, int>();
      string key;

      // Get the order frequency for each product
      foreach (var order in itemsbyOrder) {
        foreach (var item in order) {
          // Get the key by finding the name of the product whose ID matches the order items product ID
          key = GetAllProducts().Where(p => p.Id == item.ProductId).Select(p => p.Name).ToList()[0];

          // If the dictionary already contains the product, increment its value, otherwise add it to the dictionary
          if (productOrderFrequency.ContainsKey(key)) {
            productOrderFrequency[key] += 1;
          }
          else {
            productOrderFrequency.Add(key, 1);
          }
        }
      }

      return productOrderFrequency;
    }

    public IEnumerable<OrderModel> GetTodayOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetTodayOrders] Getting today's orders...");
        return GetAllOrders().Where(o => o.Date.Date == DateTime.Today).ToList();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get today's orders: {ex.Message}");
        return null;
      }
    }

    public IEnumerable<OrderModel> GetYesterdaysOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetYesterdaysOrders] Getting yesterday's orders...");
        return GetAllOrders().Where(o => o.Date.Date == DateTime.Today.AddDays(-1)).ToList();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get yesterday's orders: {ex.Message}");
        return null;
      }
    }

    public IEnumerable<OrderModel> GetLastWeekOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetLastWeekOrders] Getting orders from the last week...");

        // Get calendar info
        CultureInfo cultureInfo = new CultureInfo("en-US");
        CalendarWeekRule rule = cultureInfo.DateTimeFormat.CalendarWeekRule;
        DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

        return GetAllOrders()
          .Where(o => cultureInfo.Calendar.GetWeekOfYear(o.Date, rule, firstDayOfWeek) == cultureInfo.Calendar.GetWeekOfYear(DateTime.Today, rule, firstDayOfWeek))
          .ToList();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get yesterday's orders: {ex.Message}");
        return null;
      }
    }

    public IEnumerable<OrderModel> GetLastMonthOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetLastMonthOrders] Getting orders from the last month...");
        return GetAllOrders().Where(o => o.Date.Month == DateTime.Today.Month).ToList();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get orders from the last month: {ex.Message}");
        return null;
      }
    }

    public IEnumerable<OrderModel> GetLastYearOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetLastYearOrders] Getting orders from the last year...");
        return GetAllOrders().Where(o => o.Date.Year == DateTime.Today.Year).ToList();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get orders from the last year: {ex.Message}");
        return null;
      }
    }

    public double GetTotalDailySales() {
      try {
        logger.LogInformation("[PizzaRepository::GetTotalDailySales] Getting total daily sales...");
        return GetTodayOrders().Select(o => o.SubTotal).Sum();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get total daily sales: {ex.Message}");
        return 0;
      }
    }

    public double GetTotalWeeklySales() {
      try {
        logger.LogInformation("[PizzaRepository::GetTotalWeeklySales] Getting total weekly sales...");
        return GetLastWeekOrders().Select(o => o.SubTotal).Sum();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get total daily sales: {ex.Message}");
        return 0;
      }
    }

    public double GetTotalMonthlySales() {
      try {
        logger.LogInformation("[PizzaRepository::GetTotalMonthlySales] Getting total monthly sales...");
        return GetLastMonthOrders().Select(o => o.SubTotal).Sum();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get total monthly sales: {ex.Message}");
        return 0;
      }
    }

    public double GetTotalYearlySales() {
      try {
        logger.LogInformation("[PizzaRepository::GetTotalYearlySales] Getting total yearly sales...");
        return GetLastYearOrders().Select(o => o.SubTotal).Sum();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get total monthly sales: {ex.Message}");
        return 0;
      }
    }

    public int GetTotalDailyOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetTotalDailyOrder] Getting total daily orders...");
        return GetTodayOrders().Count();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get total daily orders: {ex.Message}");
        return 0;
      }
    }

    public int GetTotalWeeklyOrder() {
      try {
        logger.LogInformation("[PizzaRepository::GetTotalWeeklyOrder] Getting total weekly orders...");
        return GetLastWeekOrders().Count();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get total weekly orders: {ex.Message}");
        return 0;
      }
    }

    public int GetTotalMonthlyOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetTotalMonthlyOrder] Getting total monthly orders...");
        return GetLastMonthOrders().Count();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get total monthly orders: {ex.Message}");
        return 0;
      }
    }

    public int GetTotalYearlyOrders() {
      try {
        logger.LogInformation("[PizzaRepository::GetTotalYearlyOrder] Getting total yearly orders...");
        return GetLastYearOrders().Count();
      }
      catch (Exception ex) {
        logger.LogError($"Failed to get total yearly orders: {ex.Message}");
        return 0;
      }
    }
  }
}
