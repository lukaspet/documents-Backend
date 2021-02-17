using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class DocFile
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string UniquefileName { get; set; }
        public int Master { get; set; }
        public int Position { get; set; }
        public int? DocumentId { get; set; }
    }
}
