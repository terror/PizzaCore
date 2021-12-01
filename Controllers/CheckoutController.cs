using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data.Entities;
using System;
using PizzaCore.Data;
using PizzaCore.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace PizzaCore.Controllers {
  public class CheckoutController : Controller {
    private readonly IPizzaCoreRepository repository;
    private readonly ILogger<CheckoutController> logger;

    public CheckoutController(IPizzaCoreRepository repository, ILogger<CheckoutController> logger) {
      this.repository = repository;
      this.logger = logger;
    }

    // GET: /checkout
    public IActionResult Index() {
      IEnumerable<CartItem> cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(item => item.ProductSize.Price * item.Quantity) : default;
      return View();
    }

    // POST: /checkout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(OrderModel order) {
      // TODO: Save cart items, process payment
      repository.SaveOrder(order.setDate(DateTime.Now));
      return View("Success", order);
    }
  }
}
