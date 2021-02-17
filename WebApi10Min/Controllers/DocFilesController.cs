using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WebApi10Min.Models;

namespace WebApi10Min.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[Route("[controller]")]
    [ApiController]
    public class DocFilesController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DocFilesController(MyDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/DocFiles
        //[HttpGet]
        //public IEnumerable<DocFile> GetDocFile()
        //{
        //    return _context.DocFile;
        //}

        // GET: api/DocFiles/5 - download single file
        //[AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var file = await _context.DocFile.Where(x => x.FileId == id).FirstOrDefaultAsync();
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(uploads, file.UniquefileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            Response.Headers["Content-Disposition"] = "inline; filename=" + file.FileName;
            return File(memory, GetContentType(file.FileName));
        }
        // GET: api/DocFiles/list/5
        [HttpGet] //("{id}")]
        [Route("currentDocument/{id}")]
        public async Task<IActionResult> GetDocFilesList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var docFile = await _context.DocFile.Where(x => x.DocumentId == id).ToListAsync();

            if (docFile == null)
            {
                return NotFound();
            }

            return Ok(docFile);
        }
        // PUT: api/DocFiles/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDocFile([FromRoute] int id, [FromBody] DocFile docFile)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != docFile.FileId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(docFile).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DocFileExists(id))
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

        // POST: api/DocFiles
        [Authorize(Roles = "Admin, Operator, OperatorSpa")]
        [HttpPost]
        public async Task<IActionResult> PostDocFile([FromForm] DocFile docFileAdd, IFormFile file)
        {
            string uniqueFileName = null;

            DocFile df = new DocFile();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (file != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString();
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                df.FileName = file.FileName;
                df.UniquefileName = uniqueFileName;
                df.Path = filePath;
                df.Position = docFileAdd.Position;
                df.DocumentId = docFileAdd.DocumentId;
                df.Master = docFileAdd.Master;
                
                _context.DocFile.Add(df);
                await _context.SaveChangesAsync();
            }      
            return CreatedAtAction("GetDocFile", new { id = df.FileId }, df);
        }

        // DELETE: api/DocFiles/5
        [Authorize(Roles = "Admin, Operator, OperatorSpa")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var docFile = await _context.DocFile.FindAsync(id);
            if (docFile == null)
            {
                return NotFound();
            }

            var fullPath = docFile.Path;
            
            if (System.IO.File.Exists(fullPath))
            {
                try //Maybe error could happen like Access denied or Presses Already User used
                {
                    System.IO.File.Delete(fullPath);
                    _context.DocFile.Remove(docFile);
                    await _context.SaveChangesAsync();
                    return Ok(docFile);
                }
                catch (Exception e)
                {
                    return NotFound(e.ToString());
                    //Debug.WriteLine(e.Message);
                }
            }
            _context.DocFile.Remove(docFile);
            await _context.SaveChangesAsync();
            return NotFound("Impossibile trovare il file sul disco! File rimosso dall database!");
        }

        private bool DocFileExists(int id)
        {
            return _context.DocFile.Any(e => e.FileId == id);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}