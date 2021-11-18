using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Data {
  public class PizzaCoreContext : DbContext {
    public DbSet<ContactModel> ContactModel { get; set; }
    public DbSet<MenuModel> MenuModel { get; set; }

    public PizzaCoreContext(DbContextOptions<PizzaCoreContext> options) : base(options) { }


    public PizzaCoreContext() : base()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
      optionBuilder.UseSqlServer(
          @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog=PizzaCoreDB;")
          .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
          LogLevel.Information);

    }
  }
}
