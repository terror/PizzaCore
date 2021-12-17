using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PizzaCore.Areas.Identity.Pages.Account {
  [AllowAnonymous]
  public class LogoutModel : PageModel {
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, UserManager<IdentityUser> userManager) {
      _signInManager = signInManager;
      _logger = logger;
      _userManager = userManager;
    }

    public void OnGet() {
    }

    public async Task<IActionResult> OnPost(string returnUrl = null) {
      var user = await _userManager.GetUserAsync(User);
      var role = _userManager.GetRolesAsync(user);
      await _signInManager.SignOutAsync();
      _logger.LogInformation("User logged out.");

      
      if (role.Result.Contains("Staff"))
      {
        return LocalRedirect("~/identity/account/login");
      }

      if (returnUrl != null) {
        return LocalRedirect(returnUrl);
      } else {
        return RedirectToPage();
      }
    }
  }
}
