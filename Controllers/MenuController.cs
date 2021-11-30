using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Data.Entities;
using PizzaCore.Helpers;
using System.Collections.Generic;
using System.Dynamic;
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

    [HttpPost("cart")]
    public IActionResult PostCart(int productSizeId){
      ProductSize productSize = repository.GetProductSize(productSizeId);
      repository.AddToCart(HttpContext.Session, productSize);

      return RedirectToAction("Index");
    }

    [HttpDelete("cart/{id}")]
    public IActionResult DeleteCart(int productSizeId) {
      repository.RemoveFromCart(HttpContext.Session, productSizeId);
      return RedirectToAction("Index");
    }
  }
}
