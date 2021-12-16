using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Models;
using System;
using System.Collections.Generic;

namespace PizzaCore.Controllers {
  [Authorize(Roles = "Manager,Owner")]
  public class ManageOrderController : Controller {
    private readonly IPizzaCoreRepository repository;
    private static string timeFrame;

    public ManageOrderController(IPizzaCoreRepository repository) {
      this.repository = repository;
    }

    public IActionResult Index() {
      return View(GetTimeFrameOrders());
    }

    [HttpPost]
    public ActionResult Index(string timeFrame) {
      return View(GetTimeFrameOrders());
    }

    [HttpPost]
    public RedirectResult UpdateStatus(int orderId, string status, string timeFrame) {
      // Store the time frame so that the user is returned to the same view after update is complete
      ManageOrderController.timeFrame = timeFrame.ToLower();

      // Update the order status and reload the view
      repository.UpdateOrderStatus(orderId, (Status)Enum.Parse(typeof(Status), status));
      return RedirectPermanent(HttpContext.Request.Headers["Referer"]);
    }

    [HttpPost]
    public RedirectResult CancelOrder(int orderId, string timeFrame) {
      // Store the time frame so that the user is returned to the same view after cancellation is complete
      ManageOrderController.timeFrame = timeFrame.ToLower();

      // Delete the order and reload the view
      repository.DeleteOrder(orderId);
      return RedirectPermanent(HttpContext.Request.Headers["Referer"]);
    }

    private IEnumerable<OrderModel> GetTimeFrameOrders() {
      // Get the appropriate orders based on the time frame
      switch (timeFrame ?? "today") {
        case "today":
          ViewBag.timeFrame = "Today";
          return repository.GetTodayOrders();
        case "yesterday":
          ViewBag.timeFrame = "Yesterday";
          return repository.GetYesterdaysOrders();
        case "week":
          ViewBag.timeFrame = "Week";
          return repository.GetLastWeekOrders();
        case "month":
          ViewBag.timeFrame = "Month";
          return repository.GetLastMonthOrders();
        case "year":
          ViewBag.timeFrame = "Year";
          return repository.GetLastYearOrders();
        default:
          return null;
      }
    }
  }
}
