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
using Microsoft.AspNetCore.Identity;

namespace PizzaCore.Controllers {
  public class CheckoutController : Controller {
    private readonly IPizzaCoreRepository repository;
    private readonly ILogger<CheckoutController> logger;
    private readonly IEmailSender emailSender;
    private readonly UserManager<IdentityUser> userManager;

    private const string emailTopic = "Order confirmation";

    // All valid postal code prefixes
    private List<string> postalCodes = new List<string> { "H8Y", "H9A", "H9B", "H9C", "H9H", "H9J", "H9W", "H9X", };

    public CheckoutController(IPizzaCoreRepository repository, ILogger<CheckoutController> logger, IEmailSender emailSender, UserManager<IdentityUser> userManager) {
      this.repository = repository;
      this.logger = logger;
      this.emailSender = emailSender;
      this.userManager = userManager;
    }

    // GET: /checkout
    public async Task<IActionResult> Index() {
      var user = await userManager.GetUserAsync(User);

      UserData data = user == null ? null : repository.GetUserDataByIdentityUserId(user.Id);
      if (data == null) {
        data = new UserData {
          FirstName = "",
          LastName = "",
          Address = "",
          City = "",
          PostalCode = ""
        };
      }

      IEnumerable<CartItem> cart = repository.GetCart(HttpContext.Session);
      ViewBag.cart = cart;
      ViewBag.cartTotal = cart != null ? cart.Sum(item => item.ProductSize.Price * item.Quantity) : default;
      return View(new OrderModel {
        FirstName = data.FirstName,
        LastName = data.LastName,
        Email = user == null ? "" : user.Email ?? "",
        Address = data.Address,
        City = data.City,
        PostalCode = data.PostalCode
      });
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

      
      if(order.Payment.ToLower() == "debit" || order.Payment.ToLower() == "credit") {
        order.isPaid = true;
      }
      else {
        order.isPaid = false;
      }

      // Grab the cart
      IEnumerable<CartItem> cart = repository.GetCart(HttpContext.Session);
      repository.SaveOrder(order.setDate(DateTime.Now), cart);

      // Set the order status to ordered
      order.Status = Status.Ordered;

      // Save the order
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
