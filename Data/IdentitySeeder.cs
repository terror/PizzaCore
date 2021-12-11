using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Data {
  public class IdentitySeeder {
    private readonly UserIdentityContext context;
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public IdentitySeeder(UserIdentityContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
      this.context = context;
      this.userManager = userManager;
      this.roleManager = roleManager;
    }

    public async Task SeedAsync() {
      context.Database.EnsureCreated();

      IdentityUser user = await userManager.FindByEmailAsync("test@user.com");
      if (user == null) {
        user = new IdentityUser() { UserName = "test@user.com", Email = "test@user.com" };

        var result = await userManager.CreateAsync(user, "Password123!");
        // Error out if failed to add
        if (result != IdentityResult.Success)
          throw new InvalidOperationException("Could not create new user in Identity Seeder.");

        // Add the `Admin` role
        if (!context.Roles.Any(result => result.Name == "Admin"))
          await roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "Admin" });

        await userManager.AddToRoleAsync(user, "Admin");
      }
    }
  }
}
