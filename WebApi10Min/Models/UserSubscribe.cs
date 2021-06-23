using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class UserSubscribe
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DocumentId { get; set; }
    }
}
