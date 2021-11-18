using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Data;
using PizzaCore.Models;
using PizzaCore.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace PizzaCore.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> logger;
    private readonly ReCaptcha captcha;
    private readonly PizzaCoreContext context;
    private readonly IPizzaCoreRepository repository;

    public HomeController(ILogger<HomeController> logger, ReCaptcha captcha, PizzaCoreContext context, IPizzaCoreRepository repository) {
      this.logger = logger;
      this.captcha = captcha;
      this.context = context;
      this.repository = repository;
    }

    public IActionResult Index() {
      return View();
    }

    public IActionResult About() {
      return View();
    }

    [HttpGet("contact")]
    public IActionResult Contact() {
      return View();
    }

    [HttpPost("contact")]
    public async Task<IActionResult> ContactAsync(ContactModel contact) {
      if (ModelState.IsValid) {
        var captchaResponse = Request.Form["g-recaptcha-response"].ToString();

        if (await captcha.IsValid(captchaResponse)) {
          // Add the contact to the database.
          context.Add(contact.setDate(DateTime.Now));
          await context.SaveChangesAsync();

          // Call the view Success and send the contact model
          return View("ContactSuccess", contact);
        }
      }

      return View();
    }

    [HttpGet("menu")]
    public IActionResult Menu() {
      var products = repository.GetProductsGroupedByCategory();
      return View(products);
    }

    [HttpGet("careers")]
    public IActionResult Careers() {
      return View();
    }

    [HttpPost("careers")]
    public async Task<IActionResult> CareersAsync(CareersModel careers) {
      if (ModelState.IsValid) {
        var captchaResponse = Request.Form["g-recaptcha-response"].ToString();

        if (await captcha.IsValid(captchaResponse)) {
          using (var memoryStream = new MemoryStream()) {
            await careers.CVFile.CopyToAsync(memoryStream);

            // Upload the file if less than 2 MB
            if (memoryStream.Length < 2097152) {
              // Add the career submission to the database.
              context.Add(careers.setCVBinary(memoryStream.ToArray()).setDate(DateTime.Now));
              await context.SaveChangesAsync();

              // Call the view Success and send the careers model
              return View("CareersSuccess", careers);
            } else {
              ModelState.AddModelError("File", "The file is too large.");
            }
          }
        }
      }

      return View();
    }

    [HttpGet("covid")]
    public IActionResult Covid() {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
