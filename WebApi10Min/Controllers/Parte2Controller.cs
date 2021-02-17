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
    public class Parte2Controller : ControllerBase
    {
        private readonly MyDbContext _context;

        public Parte2Controller(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Parte2
        [HttpGet]
        public IEnumerable<Parte2> GetParte2()
        {
            return _context.Parte2;
        }

        // GET: api/Parte2/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParte2([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parte2 = await _context.Parte2.FindAsync(id);

            if (parte2 == null)
            {
                return NotFound();
            }

            return Ok(parte2);
        }

        // PUT: api/Parte2/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParte2([FromRoute] int id, [FromBody] Parte2 parte2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parte2.Id)
            {
                return BadRequest();
            }

            _context.Entry(parte2).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Parte2Exists(id))
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

        // POST: api/Parte2
        [HttpPost]
        public async Task<IActionResult> PostParte2([FromBody] Parte2 parte2)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Parte2.Add(parte2);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParte2", new { id = parte2.Id }, parte2);
        }

        // DELETE: api/Parte2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParte2([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parte2 = await _context.Parte2.FindAsync(id);
            if (parte2 == null)
            {
                return NotFound();
            }
            var existsInDocument = await _context.Document.Where(e => e.Parte2Id == id).ToListAsync();
            if (existsInDocument.Count > 0)
            {
                return BadRequest("Non è possibile cancellare! Presenti record con questa Parte2!");
            }
            _context.Parte2.Remove(parte2);
            await _context.SaveChangesAsync();

            return Ok(parte2);
        }

        private bool Parte2Exists(int id)
        {
            return _context.Parte2.Any(e => e.Id == id);
        }
    }
}