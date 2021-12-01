using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Controllers
{
  public class CheckoutController : Controller
  {
    private readonly IPizzaCoreRepository repository;

    public CheckoutController(IPizzaCoreRepository repository) {
      this.repository = repository;
    }

    public IActionResult Index() {

      IEnumerable<CartItem> cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(item => item.ProductSize.Price * item.Quantity) : default;

      return View();
    }

  }
}
