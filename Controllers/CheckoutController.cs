using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data.Entities;
using System;
using PizzaCore.Data;
using PizzaCore.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  public class CheckoutController : Controller {
    private readonly IPizzaCoreRepository repository;
    private readonly ILogger<CheckoutController> logger;
    private readonly IEmailSender emailSender;

    private const string emailTopic = "Order confirmation";
    private const string emailMessage = "Thank you for placing your order at PizzaCore!\nIf you have any questions about your order, please call the store directly at (514) 457-5036";

    // All valid postal code prefixes
    private List<string> postalCodes = new List<string> { "H8Y", "H9A", "H9B", "H9C", "H9H", "H9J", "H9W", "H9X", };

    public CheckoutController(IPizzaCoreRepository repository, ILogger<CheckoutController> logger, IEmailSender emailSender) {
      this.repository = repository;
      this.logger = logger;
      this.emailSender = emailSender;
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
    public async Task<IActionResult> IndexAsync(OrderModel order) {
      // Validate postal code
      if (order.PostalCode != null && !postalCodes.Any(prefix => order.PostalCode.StartsWith(prefix)))
        return View("Error", new ErrorModel {
          Message = $"Invalid order location. Valid postal code prefixes include: {string.Join(", ", postalCodes.ToArray())}" }
        );

      // TODO: Save cart items, process payment
      repository.SaveOrder(order.setDate(DateTime.Now));

      // Send confirmation email to customer
      await emailSender.SendEmailAsync(order.Email, emailTopic, emailMessage);

      return View("Success", order);
    }
  }
}
