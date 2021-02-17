using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi10Min.Models
{
    public class DocumentFileFetch
    {
        //public int CategoriaId { get; set; }
        //public int SottocategoriaId { get; set; }
        //public int SocietaId { get; set; }
        //public string Parte2 { get; set; }
        //public string Parte3 { get; set; }
        //public string DescrizioneDocumento { get; set; }
        //public DateTime DataDocumento { get; set; }
        //public DateTime DataArchivio { get; set; }
        //public string Cartella { get; set; }
        //public string Sottocartella { get; set; }
        //public string Note { get; set; }
        public int DocumentId { get; set; }
        public int Master { get; set; }
        public int Position { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
