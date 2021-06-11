using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WebApi10Min.Helpers;
using WebApi10Min.Models;

namespace WebApi10Min.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[Route("[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        // private readonly long _fileSizeLimit; //TO DO
        // private readonly string _targetFilePath; //TO DO

        public DocumentsController(MyDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Documents
        [HttpGet]
        public async Task<IActionResult> GetDocument()
        {
            DateTime data = DateTime.Today.AddMonths(-1);
            
            int totalCount = _context.Document.Count();
            var document = await _context.Document.OrderByDescending(x => x.DataDocumento).Take(20).ToListAsync(); //await _context.Document.Where(x => x.DataArchivio > data).ToListAsync();

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(totalCount));
            return Ok(document);
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var document = await _context.Document.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }
            return Ok(document);
        }
        // GET: api/search/Documents
        [HttpGet] //("{id}")]
        [Route("search")]
        public async Task<IActionResult> GetSearchDocument([FromQuery] SearchDocument SearchDocument)//int catId, int subcatId, int officeId, int companyId, string date)
        {
            //var items = await _context.Document.Where(x => EF.Functions.FreeText(x.DescrizioneDocumento, SearchDocument.SearchText)).ToListAsync();  
            if (SearchDocument.SearchText != null)
            {
                SearchDocument.SearchText = SearchDocument.SearchText.TrimEnd();

                SearchDocument.SearchText = string.Join(" ", SearchDocument.SearchText.Split(' ').Select(w => string.Format("+{0}*", w)));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SearchDocument.EndDate = SearchDocument.EndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<Document> document;
            int totalCount;

            if (SearchDocument.SearchText != null)
            {
                document = await _context.Document.FromSql("CALL FullTextSearch({0})", SearchDocument.SearchText).ToListAsync();
            }
            else
            {
                document = await _context.Document.ToListAsync();
            }

            if (SearchDocument.CategoriaId != 0 || SearchDocument.SottocategoriaId != 0 || SearchDocument.SocietaId != 0 || SearchDocument.UfficioId != 0 || SearchDocument.StartDate != new DateTime(0001, 1, 1) || SearchDocument.Parte2Id != 0 || SearchDocument.Parte3Id != 0)
            {
                if (SearchDocument.CategoriaId != 0)
                {
                    document = document.Where(x => x.CategoriaId == SearchDocument.CategoriaId).ToList();
                }
                if (SearchDocument.SottocategoriaId != 0)
                {
                    document = document.Where(x => x.SottocategoriaId == SearchDocument.SottocategoriaId).ToList();
                }
                if (SearchDocument.SocietaId != 0)
                {
                    document = document.Where(x => x.SocietaId == SearchDocument.SocietaId).ToList();
                }
                if (SearchDocument.UfficioId != 0)
                {
                    document = document.Where(x => x.UfficioId == SearchDocument.UfficioId).ToList();
                }
                if (SearchDocument.Parte2Id != 0)
                {
                    document = document.Where(x => x.Parte2Id == SearchDocument.Parte2Id).ToList();
                }
                if (SearchDocument.Parte3Id != 0)
                {
                    document = document.Where(x => x.Parte3Id == SearchDocument.Parte3Id).ToList();
                }
                if (SearchDocument.StartDate != new DateTime(0001, 1, 1))
                {
                    document = document.Where(x => x.DataDocumento >= SearchDocument.StartDate.Date && x.DataDocumento <= SearchDocument.EndDate).ToList();
                }
            }
            int p = (SearchDocument.CurrentPage * 20) - 20; 
            totalCount = document.Count();
            if (SearchDocument.SortColumn != null && SearchDocument.SortDirection != null)
            {
                switch (SearchDocument.SortColumn)
                {
                    case "dataScadenza":
                        if (SearchDocument.SortDirection == "asc")
                        {
                            document = document.OrderBy(x => x.DataScadenza).Skip(p).Take(20).ToList();
                        }
                        else
                        {
                            document = document.OrderByDescending(x => x.DataScadenza).Skip(p).Take(20).ToList();
                        }
                        break;
                    case "dataDocumento":
                        if (SearchDocument.SortDirection == "asc")
                        {
                            document = document.OrderBy(x => x.DataDocumento).Skip(p).Take(20).ToList();
                        }
                        else
                        {
                            document = document.OrderByDescending(x => x.DataDocumento).Skip(p).Take(20).ToList();
                        }
                        break;
                    default:
                        document = document.OrderByDescending(x => x.DataDocumento).Skip(p).Take(20).ToList();
                        break;
                }
            } 
            else
            {
                document = document.OrderByDescending(x => x.DataDocumento).Skip(p).Take(20).ToList();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(totalCount));
            return Ok(document);
        }

        // PUT: api/Documents/5
        [Authorize(Roles = "Admin, Operator, OperatorSpa")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument([FromRoute] int id, [FromBody] Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.DocumentId)
            {
                return BadRequest();
            }

            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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

        // POST: api/Documents
        [Authorize(Roles = "Admin, Operator, OperatorSpa")]
        [HttpPost]
        public async Task<IActionResult> PostDocument([FromForm] Document document,List<IFormFile> files, IFormFile file)
        {
            string uniqueFileName = null;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Document.Add(document);
            await _context.SaveChangesAsync();
            Console.WriteLine(document);
            if (file != null) // save one file
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
                // file.CopyTo(new FileStream(filePath, FileMode.Create));

                DocFile df = new DocFile
                {
                    FileName = file.FileName,
                    UniquefileName = uniqueFileName,
                    Path = filePath,
                    Position = 1,
                    DocumentId = document.DocumentId,
                    Master = 1
                };
                _context.DocFile.Add(df);
                await _context.SaveChangesAsync();
            }
            if (files != null && files.Count > 0) // save many files
            {
                int count=2;
                foreach (IFormFile onefile in files)
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
                        await onefile.CopyToAsync(fileStream);
                    }
                    // onefile.CopyTo(new FileStream(filePath, FileMode.Create));

                    DocFile df = new DocFile
                    {
                        FileName = onefile.FileName,
                        UniquefileName = uniqueFileName,
                        Path = filePath,
                        Position = count,
                        DocumentId = document.DocumentId,
                    };
                    _context.DocFile.Add(df);
                    await _context.SaveChangesAsync();
                    count++;
                }
            }
            return CreatedAtAction("GetDocument", new { id = document.DocumentId }, document);
        }

        // DELETE: api/Documents/5
        [Authorize(Roles = "Admin, Operator, OperatorSpa")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            var filesToDelete = await _context.DocFile.Where(x => x.DocumentId == document.DocumentId).ToListAsync();// look for files which makes part of document and delete it!
            foreach (var fileToDelete  in filesToDelete)
            {
                _context.DocFile.Remove(fileToDelete);
                await _context.SaveChangesAsync();
            }

            _context.Document.Remove(document);
            await _context.SaveChangesAsync();

            return Ok(document);
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.DocumentId == id);
        }
    }
}