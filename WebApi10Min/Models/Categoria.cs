using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Document = new HashSet<Document>();
            Sottocategoria = new HashSet<Sottocategoria>();
        }

        public int Id { get; set; }
        public string NomeCategoria { get; set; }
        public string DescrizioneCategoria { get; set; }

        public ICollection<Document> Document { get; set; }
        public ICollection<Sottocategoria> Sottocategoria { get; set; }
    }
}
