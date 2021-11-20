using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using PizzaCore.Data;
using PizzaCore.Models;
using PizzaCore.Services;

namespace PizzaCore.Controllers {
  public class CareersController : Controller {
    private const int FILE_SIZE_LIMIT = 2097152;

    private readonly PizzaCoreContext context;
    private readonly ReCaptcha captcha;

    public CareersController(PizzaCoreContext context, ReCaptcha captcha) {
      this.context = context;
      this.captcha = captcha;
    }

    // GET: /careers
    public IActionResult Index() {
      return View();
    }

    // POST: /careers
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CareersModel careers) {
      if (ModelState.IsValid) {
        var captchaResponse = Request.Form["g-recaptcha-response"].ToString();

        if (await captcha.IsValid(captchaResponse)) {
          using (var memoryStream = new MemoryStream()) {
            await careers.CVFile.CopyToAsync(memoryStream);

            // Upload the file if less than 2 MB
            if (memoryStream.Length < FILE_SIZE_LIMIT) {
              // Add the career submission to the database.
              context.Add(careers.setCVBinary(memoryStream.ToArray()).setDate(DateTime.Now));
              await context.SaveChangesAsync();

              // Call the view Success and send the careers model
              return View("Success", careers);
            } else {
              ModelState.AddModelError("File", "The file is too large.");
            }
          }
        }
      }

      return View("Error");
    }
  }
}
