using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi10Min.Models;

namespace WebApi10Min.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[Route("[controller]")]
    [ApiController]
    public class UserSubscribesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UserSubscribesController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/UserSubscribes
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserSubscribe(string id) // get only documnets where current user is subscribed to
        {
            var userSubscribe = await _context.UserSubscribe.Where(x => x.UserId == id).ToListAsync();
            if (userSubscribe == null)
            {
                return NotFound();
            }
            return Ok(userSubscribe);
        }

        // POST: api/UserSubscribes
        [HttpPost]
        public async Task<IActionResult> PostUserSubscribe([FromBody] UserSubscribe userSubscribe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserSubscribe.Add(userSubscribe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserSubscribe", new { id = userSubscribe.Id }, userSubscribe);
        }

        // DELETE: api/UserSubscribes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSubscribe([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userSubscribe = await _context.UserSubscribe.FindAsync(id);
            if (userSubscribe == null)
            {
                return NotFound();
            }

            _context.UserSubscribe.Remove(userSubscribe);
            await _context.SaveChangesAsync();

            return Ok(userSubscribe);
        }

        private bool UserSubscribeExists(int id)
        {
            return _context.UserSubscribe.Any(e => e.Id == id);
        }
    }
}