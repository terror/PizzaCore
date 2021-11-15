using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaCore.Data;

[assembly: HostingStartup(typeof(PizzaCore.Areas.Identity.IdentityHostingStartup))]
namespace PizzaCore.Areas.Identity {
  public class IdentityHostingStartup : IHostingStartup {
    public void Configure(IWebHostBuilder builder) {
      builder.ConfigureServices((context, services) => {
      });
    }
  }
}