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
    public class Parte3Controller : ControllerBase
    {
        private readonly MyDbContext _context;

        public Parte3Controller(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Parte3
        [HttpGet]
        public IEnumerable<Parte3> GetParte3()
        {
            return _context.Parte3;
        }

        // GET: api/Parte3/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParte3([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parte3 = await _context.Parte3.FindAsync(id);

            if (parte3 == null)
            {
                return NotFound();
            }

            return Ok(parte3);
        }

        // PUT: api/Parte3/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParte3([FromRoute] int id, [FromBody] Parte3 parte3)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != parte3.Id)
            {
                return BadRequest();
            }

            _context.Entry(parte3).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Parte3Exists(id))
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

        // POST: api/Parte3
        [HttpPost]
        public async Task<IActionResult> PostParte3([FromBody] Parte3 parte3)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Parte3.Add(parte3);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParte3", new { id = parte3.Id }, parte3);
        }

        // DELETE: api/Parte3/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParte3([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parte3 = await _context.Parte3.FindAsync(id);
            if (parte3 == null)
            {
                return NotFound();
            }
            var existsInDocument = await _context.Document.Where(e => e.Parte3Id == id).ToListAsync();
            if (existsInDocument.Count > 0)
            {
                return BadRequest("Non è possibile cancellare! Presenti record con questa Parte3!");
            }
            _context.Parte3.Remove(parte3);
            await _context.SaveChangesAsync();

            return Ok(parte3);
        }

        private bool Parte3Exists(int id)
        {
            return _context.Parte3.Any(e => e.Id == id);
        }
    }
}