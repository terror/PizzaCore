using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PizzaCore.Data;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PizzaCore {
  public class Program {

    //private static readonly PizzaCoreContext _context = new PizzaCoreContext();

    public static void Main(string[] args) {
      
      /*FileStream fileStream = new FileStream("")
      MenuModel item = new MenuModel { ItemCategory = MenuModel.Category.Pizzas.ToString(), ItemDescription = "The favorite pizza of all guest, just cheese!", ItemName = "Traditional Cheese Pizza", ItemPrice = 16.45 };
      CreateMenuItem(item);*/
      CreateHostBuilder(args).Build().Run();
    }

    /*private static void CreateMenuItem(MenuModel menuItem){
      _context.Add(menuItem);
      _context.SaveChanges();
    }*/


    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
              webBuilder.UseStartup<Startup>();
            });
  }
}
