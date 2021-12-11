using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Data;
using PizzaCore.Data.Entities;
using PizzaCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  public class ProfileController : Controller {
    private readonly ILogger<ProfileController> logger;
    private readonly SignInManager<IdentityUser> signinManager;
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IPizzaCoreRepository repository;

    public ProfileController(
      ILogger<ProfileController> logger,
      SignInManager<IdentityUser> signinManager,
      UserManager<IdentityUser> userManager,
      RoleManager<IdentityRole> roleManager,
      IPizzaCoreRepository repository
    ) {
      this.logger = logger;
      this.signinManager = signinManager;
      this.userManager = userManager;
      this.roleManager = roleManager;
      this.repository = repository; ;
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Index() {
      var user = await userManager.GetUserAsync(User);
      List<string> roles = (await userManager.GetRolesAsync(user)).ToList();

      var data = repository.GetUserDataByIdentityUserId(user.Id);
      if (data == null) {
        // Create user data on first use
        data = new UserData {
          IdentityUserId = user.Id,
          Address = "Unknown",
          Orders = new List<OrderModel>()
        };
      }

      var model = new ShowProfileModel {
        UserDataId = data.UserDataId,
        Address = data.Address,
        Orders = data.Orders,
        Email = user.Email,
        Roles = roleManager.Roles.Where(x => x.Name == user.UserName).ToList()
      };

      return View(model);
    }
  }
}
