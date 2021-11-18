using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PizzaCore.Data;
using PizzaCore.Models;
using PizzaCore.Services;
using System;
using System.Drawing;
using System.IO;


namespace PizzaCore
{
  public class Startup {
    private readonly IWebHostEnvironment _env;
    private readonly PizzaCoreContext _context;

    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) {
      Configuration = configuration;
      _env = webHostEnvironment;
      _context = new PizzaCoreContext();

      // Uncomment this to add default menu items to the database.
      //addDefaultMenuItem();
    }

    private void addDefaultMenuItem()
    {
      _context.Database.EnsureCreated();

      byte[] imageByte = getMenuItemImageByte("7UP.jpg");
      MenuModel item = new MenuModel { ItemCategory = MenuModel.Category.Drinks.ToString(), ItemDescription = "Deliciously refreshing 7UP", ItemName = "Fresh 7UP", ItemPrice = 1.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("alldressed-pizza.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Pizzas.ToString(), ItemDescription = "Can't go wrong with all-dressed pizza!", ItemName = "All-dressed Pizza", ItemPrice = 16.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("beef-cheddar-burger.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Burgers.ToString(), ItemDescription = "Super filling beef burger.", ItemName = "Beef Cheddar Burger", ItemPrice = 10.89, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("big-core-burger.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Burgers.ToString(), ItemDescription = "Our speciality burger, the big core burger", ItemName = "Big Core Burger", ItemPrice = 11.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("chicken-core-burger.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Burgers.ToString(), ItemDescription = "Our speciality chicken burger, the chicken core burger", ItemName = "Chicken Core Burger", ItemPrice = 10.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("coke.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Drinks.ToString(), ItemDescription = "Deliciously refreshing coke", ItemName = "Fresh Coke", ItemPrice = 1.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("diet-coke.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Drinks.ToString(), ItemDescription = "Deliciously refreshing diet coke", ItemName = "Fresh Diet Coke", ItemPrice = 1.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("diet-pepsi.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Drinks.ToString(), ItemDescription = "Deliciously refreshing diet pepsi", ItemName = "Fresh Diet Pepsi", ItemPrice = 1.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("ginger-ale.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Drinks.ToString(), ItemDescription = "Deliciously refreshing ginger ale", ItemName = "Fresh Ginger Ale", ItemPrice = 1.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("pepsi.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Drinks.ToString(), ItemDescription = "Deliciously refreshing pepsi", ItemName = "Fresh Pepsi", ItemPrice = 1.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("salted-fries.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Fries.ToString(), ItemDescription = "Traditional salted fries for everyone", ItemName = "Salted Fries", ItemPrice = 2.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("speciality-egg-pizza.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Pizzas.ToString(), ItemDescription = "Our speciality egg pizza, try it and you will love it", ItemName = "Speciality Egg Pizza", ItemPrice = 17.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("speciality-pepperoni-pizza.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Pizzas.ToString(), ItemDescription = "For all pepperoni lovers! Can't get wrong with the traditional pepperoni pizza.", ItemName = "Traditional Pepperoni Pizza", ItemPrice = 17.45, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("super-cheese-pizza.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Pizzas.ToString(), ItemDescription = "The super cheesy pizza best for cheese lovers", ItemName = "Super Cheese Pizza", ItemPrice = 16.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("traditional cheese-pizza.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Pizzas.ToString(), ItemDescription = "The favorite pizza of all guest, just cheese!", ItemName = "Traditional Cheese Pizza", ItemPrice = 16.45, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("traditional-vegetarian-pizza.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Pizzas.ToString(), ItemDescription = "The full vegetable pack, our traditional vegetarian pizza", ItemName = "Traditional Vegetarian Pizza", ItemPrice = 18.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("unsalted-fries.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Fries.ToString(), ItemDescription = "Fresh unsalted fries for healthy folks", ItemName = "Unsalted Fries", ItemPrice = 2.99, ItemImage = imageByte };
      _context.Add(item);

      imageByte = getMenuItemImageByte("water-bottle.jpg");
      item = new MenuModel { ItemCategory = MenuModel.Category.Drinks.ToString(), ItemDescription = "Fresh water bottle to keep everyone hydrated", ItemName = "Water Bottle", ItemPrice = 1.99, ItemImage = imageByte };
      _context.Add(item);

      _context.SaveChanges();
    }

    private byte[] getMenuItemImageByte(string imageName)
    {
      var stream = _env.WebRootFileProvider.GetFileInfo("images/menuItems/" + imageName).CreateReadStream();

      byte[] imageByte;
      using (Image image = Image.FromStream(stream))
      {
        using (var ms = new MemoryStream())
        {
          image.Save(ms, image.RawFormat);
          imageByte = ms.ToArray();
        }
      }

      return imageByte;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(
              Configuration.GetConnectionString("DefaultConnection")));
      services.AddDatabaseDeveloperPageExceptionFilter();

      services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
          .AddEntityFrameworkStores<ApplicationDbContext>();
      services.AddControllersWithViews();

      services.AddDbContext<PizzaCoreContext>(options => {
        options.UseSqlServer(Configuration.GetConnectionString("PizzaCoreConnection"));
      });

      services.Configure<GoogleServicesOptions>(options => {
        options.ReCaptchaApiKey = Configuration["ExternalProviders:Google:ReCaptchaApiKey"];
        options.MapsApiKey = Configuration["ExternalProviders:Google:MapsApiKey"];
      });


      // Enforce lowercase routing
      services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
      services.AddHttpClient<ReCaptcha>(x => {
        x.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
      } else {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });
    }
  }
}
