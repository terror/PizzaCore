using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Controllers
{
  [Authorize(Roles = "Owner, Manager, Cook, Delivery, Service")]
  public class EmployeeController : Controller
  {
    private readonly IPizzaCoreRepository repository;

    public EmployeeController(IPizzaCoreRepository repository) {
      this.repository = repository;
    }

    public IActionResult Index() {

      // Add employee specific first page they should see on sign in here
      if (User.IsInRole("Service"))
      {
        return RedirectToAction("Menu", "Employee");
      }

      return RedirectToAction("Index", "Home");

    }

    [HttpGet("employee/menu")]
    public IActionResult Menu() {
      // Storing cart & cartTotal in ViewBag
      var cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(cartItem => cartItem.ProductSize.Price * cartItem.Quantity) : default;
      var products = repository.GetProductsGroupedByCategory();
      return View(products);
    }

    [HttpGet("employee/order/pickup")]
    public IActionResult PickUp() {
      return Order(false);
    }

    [HttpGet("employee/order/delivery")]
    public IActionResult Delivery()
    {
      return Order(true);
    }

    
    public IActionResult Order(bool isDelivery) {
      ViewData["isDelivery"] = isDelivery;
      var cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(cartItem => cartItem.ProductSize.Price * cartItem.Quantity) : default;

      return View("Order");
    }
  }
}
