using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Controllers
{
  [Authorize(Roles = "Delivery")]
  public class DeliveryController : Controller
  {
    private readonly IPizzaCoreRepository repository;

    public DeliveryController(IPizzaCoreRepository repository)
    {
      this.repository = repository;
    }

    public IActionResult Index()
    {
      IEnumerable<OrderModel> orderModels = repository.GetOrdersDeliveryReadyOrderByOldest();
      return View(orderModels);
    }

    public IActionResult UpdateOrderStatus(int orderId, string status)
    {
      if (status != Status.InTransit.ToString() && status != Status.Complete.ToString())
      {
        return View("Error", new ErrorModel
        {
          Message = $"Status permission not allowed!"
        });
      }
      // Update the order status and reload the view
      repository.UpdateOrderStatus(orderId, (Status)Enum.Parse(typeof(Status), status));
      return RedirectPermanent(HttpContext.Request.Headers["Referer"]);
    }
  }
}
