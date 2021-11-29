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
    private static int productSizeId = -1;

    public MenuController(IPizzaCoreRepository repository) {
      this.repository = repository;
    }

    // GET: /menu
    public IActionResult Index() {
      var cart = repository.GetCart(HttpContext.Session);
      
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(item => item.Product.Price * item.Quantity) : default;
      var products = repository.GetProductsGroupedByCategory();

      dynamic model = new ExpandoObject();
      model.TheProducts = products;
      model.TheCart = cart;


      return View(model);
    }

    [HttpPost("cart")]
    public IActionResult PostCart(int id){
      repository.AddToCart(HttpContext.Session, id);
      productSizeId = id;
      return RedirectToAction("Index");
    }

    [HttpDelete("cart/{id}")]
    public IActionResult DeleteCart(int id) {
      repository.RemoveFromCart(HttpContext.Session, id);
      return RedirectToAction("Index");
    }
  }
}
