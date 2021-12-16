using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Data.Entities;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaCore.Controllers
{
  [Authorize(Roles = "Owner, Manager, Cook, Delivery, Service")]
  public class EmployeeController : Controller
  {
    private readonly IPizzaCoreRepository repository;
    private readonly IEmailSender emailSender;

    // All valid postal code prefixes
    private List<string> postalCodes = new List<string> { "H8Y", "H9A", "H9B", "H9C", "H9H", "H9J", "H9W", "H9X", };
    private const string emailTopic = "Order confirmation";

    public EmployeeController(IPizzaCoreRepository repository, IEmailSender emailSender) {
      this.repository = repository;
      this.emailSender = emailSender;
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

    [HttpPost("employee/order/delivery")]
    public async Task<IActionResult> DeliveryAsync(OrderModel order)
    {
      return await OrderAsync(order, true);
    }

    [HttpPost("employee/order/pickup")]
    public async Task<IActionResult> PickUpAsync(OrderModel order)
    {
      return await OrderAsync(order, false);
    }

    public IActionResult Order(bool isDelivery) {
      ViewData["isDelivery"] = isDelivery;
      var cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(cartItem => cartItem.ProductSize.Price * cartItem.Quantity) : default;

      return View("Order");
    }

    public async Task<IActionResult> OrderAsync(OrderModel order, bool isDelivery)
    {
      // Validate postal code
      if (order.PostalCode != null && !postalCodes.Any(prefix => order.PostalCode.StartsWith(prefix)))
        return View("Error", new ErrorModel
        {
          Message = $"Invalid order location. Valid postal code prefixes include: {string.Join(", ", postalCodes.ToArray())}"
        }
        );

      // TODO: Save cart items
      repository.SaveOrder(order.setDate(DateTime.Now));

      // Send confirmation email to customer
      IEnumerable<CartItem> cart = repository.GetCart(HttpContext.Session);

      StringBuilder emailMessage = new StringBuilder();
      emailMessage.Append("Thank you for placing your order at PizzaCore!<br><br>");
      emailMessage.Append("Your Order:<br>");
      foreach (CartItem item in cart)
      {
        string cost = (item.ProductSize.Price * item.Quantity).ToString("C2");
        emailMessage.Append($"{cost} | {item.Quantity} {item.ProductSize.Size} {item.ProductSize.Product.Name}<br>");
      }
      emailMessage.AppendFormat("<br>Subtotal: {0}<br>", order.SubTotal.ToString("C2"));
      //emailMessage.AppendFormat("Shipping Cost: {0}<br>", order.ShippingCost.ToString("C2"));
      emailMessage.AppendFormat("Taxes: {0}<br>", order.Taxes.ToString("C2"));
      emailMessage.AppendFormat("Total: {0}<br><br>", order.GetTotal().ToString("C2"));
      emailMessage.Append("If you have any questions about your order, please call the store directly at (514) 457-5036.");

      await emailSender.SendEmailAsync(order.Email, emailTopic, emailMessage.ToString());

      // Remove all items from the cart
      repository.ResetCart(HttpContext.Session);

      ViewData["askPayment"] = true;
      ViewData["isDelivery"] = isDelivery;

      // We don't want to ask for payment decision (Pay now / Pay later) since it is delivery
      if (isDelivery)
        return RedirectToAction("Menu", "Employee");


      return View("Order");
    }

  }
}
