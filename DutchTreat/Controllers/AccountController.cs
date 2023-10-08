using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<StoreUser> signInManager;
        private readonly UserManager<StoreUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signInManager, 
            UserManager<StoreUser> userManager, IConfiguration configuration)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                        return Redirect(Request.Query["ReturnUrl"].First());
                    else
                        return RedirectToAction("Shop", "App");
                }
            }

            ModelState.AddModelError("", "Login Failed!");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                StoreUser user = await this.userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var result = await this.signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded) // Create Token
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Token:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(this.configuration["Token:Issuer"],
                                                         this.configuration["Token:Audience"],
                                                         claims, 
                                                         signingCredentials: creds,
                                                         expires: DateTime.UtcNow.AddMinutes(20));

                        return Created("", new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
            }

            return BadRequest("Invalid authorization credentials");
        }
    }
}
