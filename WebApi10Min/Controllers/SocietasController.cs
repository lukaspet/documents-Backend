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
    public class SocietasController : ControllerBase
    {
        private readonly MyDbContext _context;

        public SocietasController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Societas
        [HttpGet]
        public IEnumerable<Societa> GetSocieta()
        {
            return _context.Societa;
        }

        // GET: api/Societas/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSocieta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var societa = await _context.Societa.FindAsync(id);

            if (societa == null)
            {
                return NotFound();
            }

            return Ok(societa);
        }

        // PUT: api/Societas/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSocieta([FromRoute] int id, [FromBody] Societa societa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != societa.Id)
            {
                return BadRequest();
            }

            _context.Entry(societa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocietaExists(id))
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

        // POST: api/Societas
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostSocieta([FromBody] Societa societa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Societa.Add(societa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSocieta", new { id = societa.Id }, societa);
        }

        // DELETE: api/Societas/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSocieta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var societa = await _context.Societa.FindAsync(id);

            var existsInDocument = await _context.Document.Where(e => e.SocietaId == id).ToListAsync();
            if (existsInDocument.Count > 0)
            {
                return BadRequest("None e possibile cancellare! Presenti record con questa Società!");
            }

            if (societa == null)
            {
                return NotFound();
            }

            _context.Societa.Remove(societa);
            await _context.SaveChangesAsync();

            return Ok(societa);
        }

        private bool SocietaExists(int id)
        {
            return _context.Societa.Any(e => e.Id == id);
        }
    }
}