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
    public class SottocategoriasController : ControllerBase
    {
        private readonly MyDbContext _context;

        public SottocategoriasController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Sottocategorias
        [HttpGet]
        public IEnumerable<Sottocategoria> GetSottocategoria()
        {
            return _context.Sottocategoria;
        }

        // GET: api/Sottocategorias/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSottocategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sottocategoria = await _context.Sottocategoria.FindAsync(id);

            if (sottocategoria == null)
            {
                return NotFound();
            }

            return Ok(sottocategoria);
        }
        // GET: api/Sottocategorias/list/5
        [Authorize(Roles = "Admin, Operator, OperatorSpa")]
        [HttpGet] //("{id}")]
        [Route("currentCategory/{id}")]
        public async Task<IActionResult> GetSottocategoriaList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sottocategoria = await _context.Sottocategoria.Where(x => x.CategoriaId == id).ToListAsync();

            if (sottocategoria == null)
            {
                return NotFound();
            }

            return Ok(sottocategoria);
        }

        // PUT: api/Sottocategorias/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSottocategoria([FromRoute] int id, [FromBody] Sottocategoria sottocategoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sottocategoria.Id)
            {
                return BadRequest();
            }

            _context.Entry(sottocategoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SottocategoriaExists(id))
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

        // POST: api/Sottocategorias
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostSottocategoria([FromBody] Sottocategoria sottocategoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Sottocategoria.Add(sottocategoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSottocategoria", new { id = sottocategoria.Id }, sottocategoria);
        }

        // DELETE: api/Sottocategorias/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSottocategoria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sottocategoria = await _context.Sottocategoria.FindAsync(id);
            if (sottocategoria == null)
            {
                return NotFound();
            }
            var existsInDocument = await _context.Document.Where(e => e.SottocategoriaId == id).ToListAsync();
            if (existsInDocument.Count > 0)
            {
                return BadRequest("Non è possibile cancellare! Presenti record con questa Sottocategoria!");
            }
            _context.Sottocategoria.Remove(sottocategoria);
            await _context.SaveChangesAsync();

            return Ok(sottocategoria);
        }

        private bool SottocategoriaExists(int id)
        {
            return _context.Sottocategoria.Any(e => e.Id == id);
        }
    }
}