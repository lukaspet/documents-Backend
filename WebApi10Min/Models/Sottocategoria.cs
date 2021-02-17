using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Sottocategoria
    {
        public Sottocategoria()
        {
            Document = new HashSet<Document>();
        }

        public int Id { get; set; }
        public string NomeSottocategoria { get; set; }
        public int CategoriaId { get; set; }
        public string DescrizioneSottocategoria { get; set; }

        public Categoria Categoria { get; set; }
        public ICollection<Document> Document { get; set; }
    }
}
