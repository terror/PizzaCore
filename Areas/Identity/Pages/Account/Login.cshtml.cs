using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PizzaCore.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PizzaCore.Areas.Identity.Pages.Account {
  [AllowAnonymous]
  public class LoginModel : PageModel {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(SignInManager<IdentityUser> signInManager,
        ILogger<LoginModel> logger,
        UserManager<IdentityUser> userManager) {
      _userManager = userManager;
      _signInManager = signInManager;
      _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel {
      [Required]
      [EmailAddress]
      public string Email { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      [Display(Name = "Remember me?")]
      public bool RememberMe { get; set; }

      [Display(Name ="Login as employee?")]
      public bool LogInAsEmployee { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null) {
      if (!string.IsNullOrEmpty(ErrorMessage)) {
        ModelState.AddModelError(string.Empty, ErrorMessage);
      }

      returnUrl ??= Url.Content("~/");

      // Clear the existing external cookie to ensure a clean login process
      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

      ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

      ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null) {
      returnUrl ??= Url.Content("~/");

      ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

      if (ModelState.IsValid) {
        var user = await _userManager.FindByEmailAsync(Input.Email);
        var role = _userManager.GetRolesAsync(user);

        // Don't let non-employee log in as employee
        if (Input.LogInAsEmployee && !role.Result.Contains("Staff")) {
          ModelState.AddModelError(string.Empty, "Invalid login attempt: You do not have an employee account.");
          return Page();
        }


        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded) {
          _logger.LogInformation("User logged in.");

          ISession session = HttpContext.Session;

          /*var claims = await _signInManager.UserManager.GetClaimsAsync(user);
          foreach(Claim claim in claims) {
            if(claim.Type == "isEmployeeSignIn") {
              await _signInManager.UserManager.RemoveClaimAsync(user, claim);
            }
          }*/

          SessionHelper.SetObjectAsJson(session, SessionHelper.EMPLOYEE_SIGN_IN_KEY, Input.LogInAsEmployee);
         /* await _signInManager.UserManager.AddClaimAsync(user, new Claim("isEmployeeSignIn", Input.LogInAsEmployee.ToString()));
          
          bool isTheUserAnEmployee = claims.Any(c => c.Type == "isEmployeeSignIn" && c.Value == true.ToString());

          bool test = User.HasClaim("isEmployeeSignIn", true.ToString());
          bool test2 = User.HasClaim("isEmployeeSignIn", false.ToString());*/

          // Redirect the user to the appropriate page depending on their role
          if (Input.LogInAsEmployee)
          {
            if (role.Result.Contains("Owner") || role.Result.Contains("Manager"))
            {
              return LocalRedirect("~/dashboard");
            }
            else if (role.Result.Contains("Cook"))
            {
              return LocalRedirect("~/cook");
            }
            else if (role.Result.Contains("Delivery"))
            {
              return LocalRedirect("~/delivery");
            }
            else if (role.Result.Contains("Service"))
            {
              return LocalRedirect("~/employee");
            }
          }
          

          return LocalRedirect(returnUrl);
        }
        if (result.RequiresTwoFactor) {
          return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        }
        if (result.IsLockedOut) {
          _logger.LogWarning("User account locked out.");
          return RedirectToPage("./Lockout");
        } else {
          ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          return Page();
        }
      }

      // If we got this far, something failed, redisplay form
      return Page();
    }
  }
}
