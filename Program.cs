using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PizzaCore.Data;

namespace PizzaCore {
  public class Program {
    public static void Main(string[] args) {
      Seed(CreateHostBuilder(args).Build()).Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => {
          webBuilder.UseStartup<Startup>();
        });

    private static IHost Seed(IHost host) {
      var scopeFactory = host.Services.GetService<IServiceScopeFactory>();

      using (var scope = scopeFactory.CreateScope()) {
        var seeder = scope.ServiceProvider.GetService<PizzaCoreSeeder>();
        seeder.Seed();
      }

      return host;
    }
  }
}
