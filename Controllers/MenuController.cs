using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Data.Entities;
using System.Linq;

namespace PizzaCore.Controllers {
  public class MenuController : Controller {
    private readonly IPizzaCoreRepository repository;

    public MenuController(IPizzaCoreRepository repository) {
      this.repository = repository;
    }

    // GET: /menu
    public IActionResult Index() {
      // Storing cart & cartTotal in ViewBag
      var cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(cartItem => cartItem.ProductSize.Price * cartItem.Quantity) : default;
      var products = repository.GetProductsGroupedByCategory();
      return View(products);
    }

    // POST: /cart
    [HttpPost("cart")]
    public IActionResult PostCart(int productSizeId) {
      ProductSize productSize = repository.GetProductSize(productSizeId);
      repository.AddToCart(HttpContext.Session, productSize);
      return RedirectPermanent(HttpContext.Request.Headers["Referer"]);
    }

    [HttpPost("cartItem")]
    public IActionResult DeleteCart(int productSizeId) {
      //var product = productSize;
      repository.RemoveFromCart(HttpContext.Session, productSizeId);
      return RedirectPermanent(HttpContext.Request.Headers["Referer"]);
    }

    [HttpPost("cartEdit")]
    public IActionResult EditCart(int Quantity, int ProductSizeId)
    {
      CartItem updatedCartItem = new CartItem() { ProductSize = repository.GetProductSize(ProductSizeId), Quantity = Quantity };
      repository.UpdateCart(HttpContext.Session, updatedCartItem);
      return RedirectPermanent(HttpContext.Request.Headers["Referer"]);
    }
  }
}
