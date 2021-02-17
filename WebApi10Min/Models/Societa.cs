using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Societa
    {
        public Societa()
        {
            Document = new HashSet<Document>();
        }

        public int Id { get; set; }
        public string NomeSocieta { get; set; }

        public ICollection<Document> Document { get; set; }
    }
}
