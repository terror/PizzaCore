using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using PizzaCore.Data;
using PizzaCore.Models;
using PizzaCore.Services;

namespace PizzaCore.Controllers {
  public class ContactController : Controller {
    private readonly PizzaCoreContext context;
    private readonly ReCaptcha captcha;

    public ContactController(PizzaCoreContext context, ReCaptcha captcha) {
      this.context = context;
      this.captcha = captcha;
    }

    // GET: /contact
    public IActionResult Index() {
      return View();
    }

    // POST: /contact
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactModel contact) {
      if (ModelState.IsValid) {
        var captchaResponse = Request.Form["g-recaptcha-response"].ToString();

        if (await captcha.IsValid(captchaResponse)) {
          // Add the contact to the database.
          context.Add(contact.setDate(DateTime.Now));
          await context.SaveChangesAsync();

          // Call the view Success and send the contact model
          return View("Success", contact);
        }
      }

      return View("Error");
    }
  }
}
