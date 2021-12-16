using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PizzaCore.Data {
  public class UserIdentityContext : IdentityDbContext {
    public UserIdentityContext(DbContextOptions<UserIdentityContext> options)
        : base(options) {
    }
  }
}
