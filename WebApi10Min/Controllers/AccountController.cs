using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi10Min.DTO;
using WebApi10Min.Models;
using Microsoft.EntityFrameworkCore;
using WebApi10Min.Helpers;

namespace WebApi10Min.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //private readonly IMapper _mapper;
        //private readonly AuthDbContext _authDbContext;
        private readonly UserManager<Aspnetusers> _userManager;
        private readonly SignInManager<Aspnetusers> _signInManager;
        private readonly RoleManager<Aspnetroles> _roleManager;
        //private readonly ILogger<RegisterModel> _logger;
        private readonly AuthDbContext _context;

        public AccountController(AuthDbContext context, UserManager<Aspnetusers> userManager, RoleManager<Aspnetroles> roleManager, SignInManager<Aspnetusers> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<UserHelper>> GetUsers()
        {
            var userList = await (from user in _context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      user.FullName,
                                      user.Email,
                                      RoleNames = from userRole in user.Aspnetuserroles //_userManager.GetRolesAsync(user)
                                                  join role in _context.Roles //[AspNetRoles]//
                                      on userRole.RoleId
                                      equals role.Id
                                                  select role.Name,
                                      RoleId = from userRole in user.Aspnetuserroles //_userManager.GetRolesAsync(user)
                                               join role in _context.Roles //[AspNetRoles]//
                                   on userRole.RoleId
                                   equals role.Id
                                               select role.Id,
                                  }).ToListAsync();

            var userHelper = userList.Select(p => new UserHelper
            {
                Id = p.UserId,
                FullName = p.FullName,
                Email = p.Email,
                Role = string.Join(",", p.RoleNames),
                RoleId = string.Join(",", p.RoleId)
            });
            //     return await _userManager.Users.Select(p => new UserHelper
            //     {
            //         Id = p.Id,
            //         FullName = p.FullName,
            //         Email = p.Email,
            //         Role = GetRole(p),

            //})
            //// always use the asynchronous version of EF Core extension methods
            //.ToListAsync();

            return userHelper;
        }
        [HttpPost]
        // POST: api/account/changepassword
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword ([FromBody] ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                IActionResult response = Unauthorized();
                try
                {
                    var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//User.FindFirst("sub")?.Value;
                    var user = await _userManager.FindByNameAsync(email);
                    if (user == null)
                        return StatusCode(StatusCodes.Status400BadRequest, "Errore utente!");

                    var passwordOK = await _userManager.CheckPasswordAsync(user, model.OldPassword);
                    if (!passwordOK)
                        return StatusCode(StatusCodes.Status400BadRequest, "Errore della password corrente!");
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    return StatusCode(StatusCodes.Status200OK, "La password é stata cambiata con il successo!");

                }
                catch (Exception e)
                {
                    // log the exception
                    return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] RegisterDto model)
        {
        var role = _roleManager.FindByIdAsync(model.RoleId).Result;
            if (ModelState.IsValid)
            {
                var user = new Aspnetusers { FullName = model.FullName, UserName = model.Email, Email = model.Email };

                var roleResult = await _roleManager.RoleExistsAsync(role.Name);

                if (!roleResult)
                {
                    return BadRequest("Ruolo non esiste!");
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, role.Name);
                    var newUser = new
                    {
                        id = user.Id,
                        fullName = user.FullName,
                        email = user.Email,
                        username = user.UserName,
                        roleId = role.Id,
                        role = role.Name
                    };
                    // await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(newUser);
                    // return new OkObjectResult("Account created");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return new BadRequestObjectResult( ModelState);
        }
        [Authorize(Roles = "Admin")]
        // PUT: api/Documents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] string id, [FromBody] UserHelper userHelper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userHelper.Id);
            user.Id = userHelper.Id;
            user.FullName = userHelper.FullName;
            user.UserName = userHelper.Email;
            user.Email = userHelper.Email;

            //_context.Entry(newUser).State = EntityState.Modified;
            await _userManager.UpdateAsync(user);
            
            var roles = await _userManager.GetRolesAsync(user);

            var userRole = _roleManager.Roles.Where(x => x.Id == userHelper.RoleId).FirstOrDefault();

            if (userRole.Name != roles[0])
            {
                await _userManager.RemoveFromRoleAsync(user, roles[0]);
                
                await _userManager.AddToRoleAsync(user, userRole.Name);
            }
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id);
            var rolesForUser = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return NotFound("Utente non trovato!");
            }
            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    // item should be the name of the role
                    var result = await _userManager.RemoveFromRoleAsync(user, item);
                }
            }
            await _userManager.DeleteAsync(user);

            return Ok(user);
        }
        private bool UserExists(string id)
        {
            return _context.Aspnetusers.Any(e => e.Id == id);
        }
    }
}