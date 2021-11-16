using Microsoft.EntityFrameworkCore;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Data {
  public class PizzaCoreContext : DbContext {
    public DbSet<ContactModel> ContactModel { get; set; }

    public PizzaCoreContext(DbContextOptions<PizzaCoreContext> options) : base(options) { }
  }
}
