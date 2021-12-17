using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

      if (User.IsInRole("Delivery"))
      {
        return RedirectToAction("Index", "Delivery");
      }

      return RedirectToAction("Index", "Home");

    }

    [Authorize(Roles = "Service")]
    [HttpGet("employee/menu")]
    public IActionResult Menu() {
      // Storing cart & cartTotal in ViewBag
      var cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(cartItem => cartItem.ProductSize.Price * cartItem.Quantity) : default;
      var products = repository.GetProductsGroupedByCategory();
      return View(products);
    }

    [Authorize(Roles = "Service")]
    [HttpGet("employee/order/pickup")]
    public IActionResult PickUp() {
      return Order(false);
    }

    [Authorize(Roles = "Service")]
    [HttpGet("employee/order/delivery")]
    public IActionResult Delivery()
    {
      return Order(true);
    }

    [Authorize(Roles = "Service")]
    [HttpPost("employee/order/delivery")]
    public async Task<IActionResult> DeliveryAsync(OrderModel order)
    {
      return await OrderAsync(order, true);
    }

    [Authorize(Roles = "Service")]
    [HttpPost("employee/order/pickup")]
    public async Task<IActionResult> PickUpAsync(OrderModel order)
    {
      return await OrderAsync(order, false);
    }

    [Authorize(Roles = "Service")]
    public IActionResult Order(bool isDelivery) {
      ViewData["isDelivery"] = isDelivery;
      var cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(cartItem => cartItem.ProductSize.Price * cartItem.Quantity) : default;

      return View("Order");
    }

    [Authorize(Roles = "Service")]
    public async Task<IActionResult> OrderAsync(OrderModel order, bool isDelivery)
    {
      // Validate postal code
      if (order.PostalCode != null && !postalCodes.Any(prefix => order.PostalCode.StartsWith(prefix)))
        return View("Error", new ErrorModel
        {
          Message = $"Invalid order location. Valid postal code prefixes include: {string.Join(", ", postalCodes.ToArray())}"
        }
        );
      

      // Send confirmation email to customer
      IEnumerable<CartItem> cart = repository.GetCart(HttpContext.Session);
      order.isPaid = false;
      int orderId = repository.SaveOrder(order.setDate(DateTime.Now), cart);
      if(orderId == -1) {
        return StatusCode(StatusCodes.Status500InternalServerError);
      }

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
      

      ViewData["askPayment"] = true;
      ViewData["isDelivery"] = isDelivery;

      // We don't want to ask for payment decision (Pay now / Pay later) since it is delivery
      if (isDelivery) {
        repository.ResetCart(HttpContext.Session);
        return RedirectToAction("Menu", "Employee");
      }

      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(cartItem => cartItem.ProductSize.Price * cartItem.Quantity) : default;
      ViewBag.orderId = orderId;
      return View("Order");
    }

    [Authorize(Roles = "Service")]
    [HttpGet("employee/order/paylater")]
    public IActionResult PayLater()
    {
      repository.ResetCart(HttpContext.Session);
      return RedirectToAction("Menu", "Employee");
    }

    [Authorize(Roles = "Service")]
    [HttpGet("employee/order/{orderId}")]
    public IActionResult GetOrder(int orderId)
    {
      repository.ResetCart(HttpContext.Session);
      OrderModel order = repository.GetOrderById(orderId);
      List<Product> products = new List<Product>();
      for(int i = 0; i < order.Items.Count; i++)
      {
        products.Add(repository.GetProduct(order.Items[i].ProductId));
      }

      ViewBag.Products = products;
      ViewBag.cartTotal = order.Items != null ? order.Items.Sum(item => item.Price * item.Quantity) : default;

      return View(order);
    }

    [Authorize(Roles = "Service")]
    [HttpPost("employee/order")]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateOrder(OrderModel orderModel)
    {
      if (orderModel.Payment.ToLower() == "debit" || orderModel.Payment.ToLower() == "credit" || orderModel.Payment.ToLower() == "cash")
      {
        orderModel.isPaid = true;
      }
      else
      {
        orderModel.isPaid = false;
      }

      orderModel.setDate(DateTime.Now);
      repository.UpdateOrder(orderModel);

      return RedirectToAction("Menu", "Employee");
    }

    [Authorize(Roles = "Service")]
    [HttpGet("employee/orders")]
    public IActionResult GetOrders()
    {
      IEnumerable<OrderModel> orders = repository.GetPendingPickupOrdersOrderByOldest();
      return View(orders);
    }

    public IActionResult UpdateOrderStatus(int orderId, string status)
    {
      if (status != Status.Complete.ToString())
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
