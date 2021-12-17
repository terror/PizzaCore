using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCore.Data.Entities;
using PizzaCore.Data;
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
      this.repository = repository;
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> Index() {
      // Grab the current user
      var user = await userManager.GetUserAsync(User);

      // Grab a list of the users roles
      List<string> roles = (await userManager.GetRolesAsync(user)).ToList();

      // Populate user data if it doesn't exist
      var data = repository.GetUserDataByIdentityUserId(user.Id) ?? 
        new UserData {
          IdentityUserId = user.Id,
          FirstName = "",
          LastName = "",
          Address = "",
          City = "",
          PostalCode = "",
          Orders = new List<OrderModel>()
        };

      // Show the view with the constructed model
      return View(new ShowProfileModel {
        UserDataId = data.UserDataId,
        FirstName = data.FirstName,
        LastName = data.LastName,
        Email = user.Email,
        Address = data.Address,
        City = data.City,
        PostalCode = data.PostalCode,
        Orders = data.Orders,
        Roles = roleManager.Roles.Where(x => x.Name == user.UserName).ToList()
      });
    }

    [Authorize]
    [HttpPost("profile")]
    public async Task<IActionResult> Index(ShowProfileModel model) {
      // Grab the current user
      var user = await userManager.GetUserAsync(User);

      repository.SetUserDataByUserId(user.Id, model.ToUserData(user.Id));

      return View();
    }
  }
}
