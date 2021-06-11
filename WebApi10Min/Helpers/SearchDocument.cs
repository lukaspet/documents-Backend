using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi10Min.Helpers
{
    public partial class SearchDocument
    {
        public int CategoriaId { get; set; }
        public int SottocategoriaId { get; set; }
        public int SocietaId { get; set; }
        public int UfficioId { get; set; }
        public int Parte2Id { get; set; }
        public int Parte3Id { get; set; }
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
