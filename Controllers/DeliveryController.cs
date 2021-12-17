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
  }
}
