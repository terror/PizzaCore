using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PizzaCore.Data.Entities;
using PizzaCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public Product GetProduct(int id){
      try{ 
        return context.Products.Find(id);
      }
      catch (Exception ex){
        logger.LogError($"Failed to get product of id {id} : {ex.Message}");
        return null;
      }
    }

    public IEnumerable<CartItem> GetCart(ISession session){

      // get cart from session (null if cart does not exist)
      return session.GetObjectFromJson<List<CartItem>>(SESSION_KEY_CART);
    }

    public void AddToCart(ISession session, int productId)
    {
      try
      {
        logger.LogInformation("[PizzaRepository::AddToCart] Adding product to cart...");

        // Get the cart & the product we want to add
        var cartItemList = GetCart(session);
        List<CartItem> cart;
        Product productToAdd = GetProduct(productId);

        // Validate product id
        if (productToAdd == null)
          throw new ArgumentException($"Product of id {productId} does not exist", "productId");

        // If the cart doesn't exist in the session yet
        if (cartItemList == null)
        {
          cart = new List<CartItem>();
          cart.Add(new CartItem
          {
            Product = GetProduct(productId),
            Quantity = 1    // quantity is 1 because cart doesn't exist in session,
                            // so there is no product of that type in the cart yet.
          });


        }

        // If the cart already exists in the session
        else
        {
          cart = cartItemList.ToList();
          int productCartIndex = FindProductInCart(cart, productId);

          if (productCartIndex != -1)
          {
            cart[productCartIndex].Quantity++;    // Increment quantity because same
                                                  // product already exists in cart
          }
          else
          {
            cart.Add(new CartItem
            {
              Product = productToAdd,
              Quantity = 1    // quantity is 1 because no same product is in cart
            });
          }
        }

        // Set the new cart with added product in the session.
        session.SetObjectAsJson(SESSION_KEY_CART, cart);


      }
      catch (Exception ex)
      {
        logger.LogError($"Failed to add product of id {productId} to cart : {ex.Message}");
      }

    }

    public void RemoveFromCart(ISession session, int productId) {
      try {
        logger.LogInformation("[PizzaRepository::RemoveFromCart] Removing product from cart");

        // Get cart and remove product
        List<CartItem> cart = GetCart(session).ToList();
        int productCartIndex = FindProductInCart(cart, productId);
        cart.RemoveAt(productCartIndex);
      }
      catch (ArgumentOutOfRangeException) {
        logger.LogError($"Failed to remove product of id {productId} from cart : Product of id {productId} does not exist in the cart");
      }
      catch(Exception ex) {
        logger.LogError($"Failed to remove product of id {productId} from cart: {ex.Message}");
      }
      
    }

    private int FindProductInCart(List<CartItem> cart, int productId)
    {
      return cart.FindIndex(item => item.Product.Id == productId);

    }

    public bool SaveAll() {
      return context.SaveChanges() > 0;
    }
  }
}
