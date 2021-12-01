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

    private string emailTopic = "Order confirmation";
    private string emailMessage = "Thank you for placing your order at PizzaCore!\n" +
                                  "If you have any questions about your order, please call the store directly at (514) 457-5036";

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
      // TODO: Save cart items, process payment
      repository.SaveOrder(order.setDate(DateTime.Now));

      // Send confirmation email to customer
      await emailSender.SendEmailAsync(order.Email, emailTopic, emailMessage);

      return View("Success", order);
    }
  }
}
