using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger) {
      _logger = logger;
    }

    public IActionResult Index() {
      return View();
    }

    public IActionResult About() {
      return View();
    }

    public IActionResult Privacy() {
      return View();
    }

    public IActionResult Menu()
    {
      return View();
    }

   /* [HttpGet]
    public async Task<ActionResult<IEnumerable<Menu>>> GetMenu()
    {
      return await _context.Samurais.ToListAsync();
    }*/

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
