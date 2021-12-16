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
using System.Text;

namespace PizzaCore.Controllers {
  public class CheckoutController : Controller {
    private readonly IPizzaCoreRepository repository;
    private readonly ILogger<CheckoutController> logger;
    private readonly IEmailSender emailSender;

    private const string emailTopic = "Order confirmation";

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

      IEnumerable<CartItem> cart = repository.GetCart(HttpContext.Session);
      repository.SaveOrder(order.setDate(DateTime.Now), cart);

      // Send confirmation email to customer
      StringBuilder emailMessage = new StringBuilder();
      emailMessage.Append("Thank you for placing your order at PizzaCore!<br><br>");
      emailMessage.Append("Your Order:<br>");
      foreach (CartItem item in cart) {
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

      return View("Success", order);
    }
  }
}
