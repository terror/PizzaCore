using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
      await SeedRoles();
      await SeedUsers();
    }

    public async Task SeedRoles() {
      // If roles exist, don't seed
      if (await context.Roles.AnyAsync())
        return;

      // Define all roles
      var roles = new string[] { "Owner", "Manager", "Cook", "Delivery", "Service", "Staff"};

      // Create all roles
      foreach (var roleName in roles) {
        var role = await roleManager.RoleExistsAsync(roleName);
        // Create the role if it doesn't exist
        if (!role)
          await roleManager.CreateAsync(new IdentityRole { Name = roleName });
      }
    }

    public async Task SeedUsers() {
      context.Database.EnsureCreated();
      await CreateUser("owner@example.com", "Password123!", "Owner", "Service", "Staff");
      await CreateUser("manager@example.com", "Password123!", "Manager", "Service", "Staff");
      await CreateUser("cook@example.com", "Password123!", "Cook", "Staff");
      await CreateUser("delivery@example.com", "Password123!", "Delivery", "Staff");
      await CreateUser("service@example.com", "Password123!", "Service", "Staff");
    }

    private async Task CreateUser(string email, string password, params string[] roles) {
      var user = new IdentityUser { UserName = email, Email = email };
      if ((await userManager.FindByEmailAsync(email)) == null) {
        var createdUser = await userManager.CreateAsync(user, password);
        if (createdUser.Succeeded)
        {
          foreach(string role in roles)
          {
            await userManager.AddToRoleAsync(user, role);
          }
        }
          
      }
    }
  }
}
