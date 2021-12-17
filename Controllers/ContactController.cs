using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Data.Entities;
using PizzaCore.Models;
using PizzaCore.Services;
using System;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  public class ContactController : Controller {
    private readonly PizzaCoreContext context;
    private readonly ReCaptcha captcha;
    private readonly IPizzaCoreRepository repository;
    private readonly UserManager<IdentityUser> userManager;

    public ContactController(PizzaCoreContext context, ReCaptcha captcha, IPizzaCoreRepository repository, UserManager<IdentityUser> userManager) {
      this.context = context;
      this.captcha = captcha;
      this.repository = repository;
      this.userManager = userManager;
    }

    // GET: /contact
    public async Task<IActionResult> Index() {
      var user = await userManager.GetUserAsync(User);

      UserData data = user == null ? null : repository.GetUserDataByIdentityUserId(user.Id);
      if (data == null) {
        data = new UserData {
          FirstName = "",
          LastName = "",
        };
      }

      return View(new ContactModel {
        FirstName = data.FirstName,
        LastName = data.LastName,
        Email = user == null ? "" : user.Email ?? ""
      });
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
