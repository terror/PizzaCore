using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PizzaCore.Data;
using PizzaCore.Services;
using System;

namespace PizzaCore {
  public class Startup {
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      // Add and configure the application database context
      services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
      );

      // Add and configure the PizzaCore database context
      services.AddDbContext<PizzaCoreContext>(options => {
        options.UseSqlServer(Configuration.GetConnectionString("PizzaCoreConnection"));
      });

      // Configure user options
      services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
          .AddEntityFrameworkStores<ApplicationDbContext>();

      // Add a recaptcha http client service
      services.AddHttpClient<ReCaptcha>(x => {
        x.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
      });

      // Add IPizzaCoreRepository as a service with the implementation being PizzaCoreRepository
      services.AddScoped<IPizzaCoreRepository, PizzaCoreRepository>();

      // Add the database seeder
      services.AddTransient<PizzaCoreSeeder>();

      // Prettify errors in development
      services.AddDatabaseDeveloperPageExceptionFilter();

      // Configure session options
      services.AddDistributedMemoryCache();
      services.AddSession(options => {
        options.IdleTimeout = TimeSpan.FromHours(4);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
      });

      // Configure MVC services for controllers with views
      services.AddControllersWithViews();

      // Configure Google service options
      services.Configure<GoogleServicesOptions>(options => {
        options.ReCaptchaApiKey = Configuration["ExternalProviders:Google:ReCaptchaApiKey"];
        options.MapsApiKey = Configuration["ExternalProviders:Google:MapsApiKey"];
      });

      // Configure SendGrud service options
      services.AddTransient<IEmailSender, SendGridEmailSender>();
      services.Configure<SendGridEmailSenderOptions>(options => {
        options.ApiKey = Configuration["ExternalProviders:SendGrid:ApiKey"];
        options.SenderEmail = Configuration["ExternalProviders:SendGrid:SenderEmail"];
        options.SenderName = Configuration["ExternalProviders:SendGrid:SenderName"];
      });

      // Configure Google 3rd party sign in
      services.AddAuthentication()
        .AddGoogle(options => {
          IConfigurationSection googleAuthNSection =
              Configuration.GetSection("Authentication:Google");

          options.ClientId = googleAuthNSection["ClientId"];
          options.ClientSecret = googleAuthNSection["ClientSecret"];
        });

      // Enforce lowercase routing
      services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
      } else {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseSession();
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
