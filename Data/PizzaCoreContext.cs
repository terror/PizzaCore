using Microsoft.EntityFrameworkCore;
using PizzaCore.Models;

namespace PizzaCore.Data {
  public class PizzaCoreContext : DbContext {
    public DbSet<ContactModel> ContactModel { get; set; }

    public PizzaCoreContext(DbContextOptions<PizzaCoreContext> options) : base(options) { }
  }
}
