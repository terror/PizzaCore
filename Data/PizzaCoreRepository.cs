using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PizzaCore.Data.Entities;
using PizzaCore.Helpers;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
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

    public void SaveOrder(OrderModel order) {
      try {
        logger.LogInformation("[PizzaCoreRepository::SaveOrder] Saving order...");
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

    public bool SaveAll() {
      return context.SaveChanges() > 0;
    }

    public void ResetCart(ISession session) {
      session.Remove(SESSION_KEY_CART);
    }
  }
}
