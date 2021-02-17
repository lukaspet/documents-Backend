using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Document
    {
        public int DocumentId { get; set; }
        public int CategoriaId { get; set; }
        public int SottocategoriaId { get; set; }
        public int SocietaId { get; set; }
        public int UfficioId { get; set; }
        public uint? Parte2Id { get; set; }
        public uint? Parte3Id { get; set; }
        public string DescrizioneDocumento { get; set; }
        public DateTime DataDocumento { get; set; }
        public DateTime DataArchivio { get; set; }
        public string Cartella { get; set; }
        public string PosizioneCartella { get; set; }
        public string Note { get; set; }

        public Categoria Categoria { get; set; }
        public Societa Societa { get; set; }
        public Sottocategoria Sottocategoria { get; set; }
        public Ufficio Ufficio { get; set; }
    }
}
