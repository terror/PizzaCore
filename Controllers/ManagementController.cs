using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  [Authorize(Roles = "Owner, Manager")]
  public class ManagementController : Controller {
    private readonly UserManager<IdentityUser> userManager;
    private readonly IPizzaCoreRepository repository;
    private string[] roles = new string[] { "Delivery", "Service", "Cook" };

    public ManagementController(UserManager<IdentityUser> userManager, IPizzaCoreRepository repository) {
      this.userManager = userManager;
      this.repository = repository;
    }

    // GET: Management
    public async Task<IActionResult> Index() {
      var users = new List<IdentityUser>();
      foreach (var role in roles)
        foreach (var user in await userManager.GetUsersInRoleAsync(role))
          users.Add(user);
      return View(users);
    }

    // GET: Management/Create
    public IActionResult Create() {
      return View();
    }

    // POST: Management/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ManagementModel model) {
      if (ModelState.IsValid) {
        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        if (await userManager.FindByEmailAsync(model.Email) == null) {
          var createdUser = await userManager.CreateAsync(user, model.Password);
          if (createdUser.Succeeded) {
            await userManager.AddToRoleAsync(user, "Staff");
            await userManager.AddToRoleAsync(user, model.Role);
          }
          return View("Success");
        }
      }
      return View("Error");
    }

    // GET: Management/Details
    public IActionResult Details(string id) {
      return View(repository.GetUserDataByIdentityUserId(id) ?? null);
    }

    // GET: Management/Edit
    public IActionResult Edit(string id) {
      return View();
    }

    // POST: Management/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ManagementModel model) {
      if (ModelState.IsValid) {
        // Find the user
        var user = await userManager.FindByIdAsync(id);
        // Update the users roles
        if (user != null) {
          // Remove existing roles
          foreach (var role in roles)
            if (await userManager.IsInRoleAsync(user, role))
              await userManager.RemoveFromRoleAsync(user, role);
          // Re-assign
          await userManager.AddToRoleAsync(user, model.Role);
        }
        return View("Success");
      }
      return View("Error");
    }

    // GET Management/Delete
    public async Task<IActionResult> Delete(string id) {
      return View(await userManager.FindByIdAsync(id));
    }

    // POST: Management/Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(IdentityUser model) {
      // Grab user
      var user = await userManager.FindByIdAsync(model.Id);
      // If the user exists, delete it
      if (user != null) {
        // Delete associated `UserData`
        if (repository.GetUserDataByIdentityUserId(user.Id) != null)
          repository.DeleteUserDataByIdentityUserId(user.Id);
        // Delete user
        await userManager.DeleteAsync(user);
        return View("Success");
      }
      return View("Error");
    }
  }
}
