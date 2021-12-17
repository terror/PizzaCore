using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Models;
using System;
using System.Linq;

namespace PizzaCore.Controllers
{
  [Authorize(Roles = "Cook")]
  public class CookController : Controller {
    private IPizzaCoreRepository repository;

    public CookController(IPizzaCoreRepository repository) {
      this.repository = repository;
    }

    public IActionResult Index() {
      return View(repository.GetAllOrders().Where(o => o.Status == Status.Ordered || o.Status == Status.Preparing));
    }

    [HttpPost]
    public RedirectResult UpdateOrderStatus(int orderId, string status) {
      // Update the order item status and reload the view
      repository.UpdateOrderStatus(orderId, (Status)Enum.Parse(typeof(Status), status));
      return RedirectPermanent(HttpContext.Request.Headers["Referer"]);
    }

    [HttpPost]
    public RedirectResult UpdateItemStatus(int itemId, string status) {
      // Update the order item status and reload the view
      repository.UpdateOrderItemStatus(itemId, (Status)Enum.Parse(typeof(Status), status));
      return RedirectPermanent(HttpContext.Request.Headers["Referer"]);
    }
  }
}
