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
    public class LogFrontendController : ControllerBase
    {
        private readonly MyDbContext _context;
        private IHttpContextAccessor _accessor;

        public LogFrontendController(MyDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        // GET: api/LogFrontends
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<LogFrontend> GetLogFrontend()
        {
            return _context.LogFrontend;
        }

        // GET: api/LogFrontends/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogFrontend([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logFrontend = await _context.LogFrontend.FindAsync(id);

            if (logFrontend == null)
            {
                return NotFound();
            }

            return Ok(logFrontend);
        }

        // PUT: api/LogFrontends/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutLogFrontend([FromRoute] int id, [FromBody] LogFrontend logFrontend)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != logFrontend.IdLog)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(logFrontend).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LogFrontendExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/LogFrontends
        [HttpPost]
        public async Task<IActionResult> PostLogFrontend([FromBody] LogFrontend logFrontend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            logFrontend.UserIp = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            logFrontend.DatetimeBackend = DateTime.Now;

            _context.LogFrontend.Add(logFrontend);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogFrontend", new { id = logFrontend.IdLog }, logFrontend);
        }

        // DELETE: api/LogFrontends/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteLogFrontend([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var logFrontend = await _context.LogFrontend.FindAsync(id);
        //    if (logFrontend == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.LogFrontend.Remove(logFrontend);
        //    await _context.SaveChangesAsync();

        //    return Ok(logFrontend);
        //}

        private bool LogFrontendExists(int id)
        {
            return _context.LogFrontend.Any(e => e.IdLog == id);
        }
    }
}