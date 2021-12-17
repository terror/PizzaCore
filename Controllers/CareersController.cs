using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Data.Entities;
using PizzaCore.Models;
using PizzaCore.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  public class CareersController : Controller {
    private const int FILE_SIZE_LIMIT = 2097152;

    private readonly PizzaCoreContext context;
    private readonly ReCaptcha captcha;
    private readonly IPizzaCoreRepository repository;
    private readonly UserManager<IdentityUser> userManager;

    public CareersController(PizzaCoreContext context, ReCaptcha captcha, IPizzaCoreRepository repository, UserManager<IdentityUser> userManager) {
      this.context = context;
      this.captcha = captcha;
      this.repository = repository;
      this.userManager = userManager;
    }

    // GET: /careers
    public async Task<IActionResult> Index() {
      var user = await userManager.GetUserAsync(User);

      var data = repository.GetUserDataByIdentityUserId(user.Id) ??
        new UserData {
          FirstName = "",
          LastName = "",
        };

      return View(new CareersModel {
        FirstName = data.FirstName,
        LastName = data.LastName,
        Email = user.Email ?? ""
      });
    }

    // POST: /careers
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CareersModel careers) {
      var captchaResponse = Request.Form["g-recaptcha-response"].ToString();

      if (ModelState.IsValid && await captcha.IsValid(captchaResponse)) {
        using (var memoryStream = new MemoryStream()) {
          // Copy over file contents to the memory stream
          await careers.CVFile.CopyToAsync(memoryStream);

          // Error out if the file is >= 2MB
          if (memoryStream.Length >= FILE_SIZE_LIMIT)
            return View("Error", new ErrorModel { Message = "File size cannot be greater than 2MB." });

          // Add the career submission to the database.
          context.Add(careers.setCVBinary(memoryStream.ToArray()).setDate(DateTime.Now));
          await context.SaveChangesAsync();

          // Call the view Success and send the careers model
          return View("Success", careers);
        }
      }

      return View("Error");
    }
  }
}
