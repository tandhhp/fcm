using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.ExternalAPI.GoogleAggregate;

namespace Waffle.Pages.Users.Login;

public class IndexModel : EntryPageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ISettingService _appSettingService;

    [BindProperty(SupportsGet = true)]
    public string? UserName { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? Password { get; set; }

    public Microsoft.AspNetCore.Identity.SignInResult? SignInResult;

    public IndexModel(ISettingService appSettingService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, ICatalogService catalogService) : base(catalogService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _appSettingService = appSettingService;
    }

    public ExternalAPI.GoogleAggregate.Google? Google;

    public async Task OnGetAsync()
    {
        Google = await _appSettingService.GetAsync<ExternalAPI.GoogleAggregate.Google>(nameof(Google));
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");

        if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password)) return Page();
        SignInResult = await _signInManager.PasswordSignInAsync(UserName, Password, false, false);
        if (SignInResult.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty, ClaimValueTypes.String),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole, ClaimValueTypes.String));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append(CookieKey.Token, generatedToken, cookieOptions);

            return LocalRedirect(returnUrl);
        }
        return Page();
    }
}
