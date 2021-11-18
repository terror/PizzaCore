using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(PizzaCore.Areas.Identity.IdentityHostingStartup))]
namespace PizzaCore.Areas.Identity {
  public class IdentityHostingStartup : IHostingStartup {
    public void Configure(IWebHostBuilder builder) {
      builder.ConfigureServices((context, services) => {
      });
    }
  }
}
