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
    public class UfficiosController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UfficiosController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Ufficios
        [HttpGet]
        public IEnumerable<Ufficio> GetUfficio()
        {
            return _context.Ufficio;
        }

        // GET: api/Ufficios/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUfficio([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ufficio = await _context.Ufficio.FindAsync(id);

            if (ufficio == null)
            {
                return NotFound();
            }

            return Ok(ufficio);
        }

        // PUT: api/Ufficios/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUfficio([FromRoute] int id, [FromBody] Ufficio ufficio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ufficio.Id)
            {
                return BadRequest();
            }

            _context.Entry(ufficio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UfficioExists(id))
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

        // POST: api/Ufficios
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostUfficio([FromBody] Ufficio ufficio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Ufficio.Add(ufficio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUfficio", new { id = ufficio.Id }, ufficio);
        }

        // DELETE: api/Ufficios/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUfficio([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ufficio = await _context.Ufficio.FindAsync(id);
            if (ufficio == null)
            {
                return NotFound();
            }
            var existsInDocument = await _context.Document.Where(e => e.UfficioId == id).ToListAsync();
            if (existsInDocument.Count > 0)
            {
                return BadRequest("Non è possibile cancellare! Presenti record con questo Ufficio!");
            }
            _context.Ufficio.Remove(ufficio);
            await _context.SaveChangesAsync();

            return Ok(ufficio);
        }

        private bool UfficioExists(int id)
        {
            return _context.Ufficio.Any(e => e.Id == id);
        }
    }
}