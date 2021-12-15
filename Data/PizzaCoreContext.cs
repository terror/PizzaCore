using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaCore.Data.Entities;
using PizzaCore.Models;
using System;

namespace PizzaCore.Data {
  public class PizzaCoreContext : DbContext {
    public DbSet<CareersModel> CareersModels { get; set; }
    public DbSet<ContactModel> ContactModels { get; set; }
    public DbSet<ProductSize> ProductSizes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderModel> OrderModels { get; set; }
    public DbSet<UserData> UserDatas { get; set; }

    public PizzaCoreContext(DbContextOptions<PizzaCoreContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder) {
      optionBuilder.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog=PizzaCoreDB;")
        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
        LogLevel.Information
      );
    }
  }
}
