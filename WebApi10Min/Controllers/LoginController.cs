using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi10Min.DTO;
using WebApi10Min.Helpers;
using WebApi10Min.Models;

namespace WebApi10Min.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<Aspnetusers> _userManager;
        private readonly SignInManager<Aspnetusers> _signInManager;
        private readonly RoleManager<Aspnetroles> _roleManager;
        private readonly IConfiguration _config;

        public LoginController(SignInManager<Aspnetusers> signInManager, UserManager<Aspnetusers> userManager, RoleManager<Aspnetroles> roleManager, IConfiguration config)//, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
        }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> OnPostAsync([FromBody] LoginDto model)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                IActionResult response = Unauthorized();
                try
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    if (user == null)
                        return StatusCode(StatusCodes.Status401Unauthorized, "Errore email o password");

                    var passwordOK = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (!passwordOK)
                        return StatusCode(StatusCodes.Status401Unauthorized, "Errore email o password");
                    var tokenString = await GenerateJWT(model.Email);
                    var role = await GetRole(model.Email);
                    return Ok(new
                    {
                        id = user.Id,
                        FullName = user.FullName,
                        Email = model.Email,
                        Role = role[0],// return only first role
                        Token = tokenString,
                        // refreshToken = TO DO 
                    });

                }
                catch (Exception e)
                {
                    // log the exception
                    return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
                }
            }
            return BadRequest(ModelState);
        }

        //[HttpPost("Logout")]
        //public async Task<ActionResult> Logout(UserHelper userHelper)
        //{
        //    await _signInManager.SignOutAsync();

        //    return Ok();
        //}
        [NonAction]
        private async Task<string> GenerateJWT(string UserName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AppSettings:Token"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserName),
                //new Claim(ClaimTypes.Role,userInfo.Role),
                //new Claim("role",userInfo.UserType),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            var userRoles = await GetRole(UserName);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                //var role = await _roleManager.FindByNameAsync(userRole);  // ADD  WHEN CLAIMS ARE USED
                //if (role != null)
                //{
                    //var roleClaims = await _roleManager.GetClaimsAsync(role);
                    //foreach (Claim roleClaim in roleClaims)
                    //{
                        //claims.Add(role);
                    //}
                //}
            }

            var token = new JwtSecurityToken(
                issuer: _config["AppSettings:Issuer"],
                audience: _config["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [NonAction]
        private async Task<IList<String>> GetRole(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles;
        }
    }
}