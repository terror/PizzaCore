using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaCore.Data;
using PizzaCore.Models;
using PizzaCore.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace PizzaCore.Controllers {

  public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly ReCaptcha _captcha;
    private readonly PizzaCoreContext _context;


    public HomeController(ILogger<HomeController> logger, ReCaptcha captcha, PizzaCoreContext context) {
      _logger = logger;
      _captcha = captcha;
      _context = context;
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
        var captcha = Request.Form["g-recaptcha-response"].ToString();

        if (await _captcha.IsValid(captcha)) {
          // Add the contact to the database.
          _context.Add(contact.setDate(DateTime.Now));
          await _context.SaveChangesAsync();

          // Call the view Success and send the contact model
          return View("ContactSuccess", contact);
        }
      }
      return View();
    }

    [HttpGet("menu")]
    public IActionResult Menu() {
      return View();
    }

    [Route("api/menu")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuModel>>> GetMenu() {
      return await _context.MenuModel.ToListAsync();
    }

    [Route("api/menu/pictures/{id}")]
    [HttpGet]
    public async Task<ActionResult<byte[]>> GetMenuItemPics(int id) {
      var menuItem = await _context.MenuModel.FindAsync(id);
      if (menuItem == null || menuItem.ItemImage == null)
      {
        return null;
      }

      return menuItem.ItemImage;
    }

    [HttpGet("careers")]
    public IActionResult Careers() {
      return View();
    }

    [HttpPost("careers")]
    public async Task<IActionResult> CareersAsync(CareersModel careers) {
      if (ModelState.IsValid) {
        var captcha = Request.Form["g-recaptcha-response"].ToString();

        if (await _captcha.IsValid(captcha)) {

          using (var memoryStream = new MemoryStream()) {
            await careers.CVFile.CopyToAsync(memoryStream);

            // Upload the file if less than 2 MB
            if (memoryStream.Length < 2097152) {
              // Add the career submission to the database.
              _context.Add(careers.setCVBinary(memoryStream.ToArray()).setDate(DateTime.Now));
              await _context.SaveChangesAsync();

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
