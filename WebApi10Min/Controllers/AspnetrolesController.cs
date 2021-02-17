using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi10Min.Models;

namespace WebApi10Min.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    //[Route("[controller]")]
    [ApiController]
    public class AspnetrolesController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly RoleManager<Aspnetroles> _roleManager;
        private readonly UserManager<Aspnetusers> _userManager;

        public AspnetrolesController(AuthDbContext context, RoleManager<Aspnetroles> roleManager, UserManager<Aspnetusers> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // GET: api/Aspnetroles
        [HttpGet]
        public IEnumerable<Aspnetroles> Get()
        {
            return _context.Aspnetroles;
        }

        // GET: api/Aspnetroles/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Aspnetroles
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Aspnetroles role)
        {
            //_context.Aspnetroles.Add(role);
            await _roleManager.CreateAsync(role);
            //await _context.SaveChangesAsync();
            return Ok(role);
        }

        // PUT: api/Aspnetroles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole([FromRoute] string id, [FromBody]  Aspnetroles newrole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != newrole.Id)
            {
                return BadRequest();
            }

            var role = _roleManager.FindByIdAsync(id).Result;

            if (role == null)
            {
                return BadRequest("Ruolo non esiste!");
            }

            if (role.Name != newrole.Name)
            {
                role.Name = newrole.Name;
            }

            try
            {
               await _roleManager.UpdateAsync(role);
            }

            catch
            {
                throw;
            }

            return NoContent();
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = _roleManager.FindByIdAsync(id).Result;
            if (role == null)
            {
                return NotFound("Ruolo non esiste!");
            }

            var userExists = await _userManager.GetUsersInRoleAsync(role.Name);
            if (userExists.Count > 0)
            {
                return BadRequest("Non è possibile cancellare! Presenti utenti con questo Ruolo!");
            }
            await _roleManager.DeleteAsync(role);

            return Ok(role);
        }
    }
}
