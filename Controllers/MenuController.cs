using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;

namespace PizzaCore.Controllers {
  public class MenuController : Controller {
    private readonly IPizzaCoreRepository repository;

    public MenuController(IPizzaCoreRepository repository) {
      this.repository = repository;
    }

    // GET: /menu
    public IActionResult Index() {
      var products = repository.GetProductsGroupedByCategory();
      return View(products);
    }
  }
}
