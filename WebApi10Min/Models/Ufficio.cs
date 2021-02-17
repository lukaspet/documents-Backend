using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Ufficio
    {
        public Ufficio()
        {
            Document = new HashSet<Document>();
        }

        public int Id { get; set; }
        public string NomeUfficio { get; set; }

        public ICollection<Document> Document { get; set; }
    }
}
