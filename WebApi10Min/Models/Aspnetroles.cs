using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Aspnetroles : IdentityRole
    {
        public Aspnetroles()
        {
            Aspnetroleclaims = new HashSet<Aspnetroleclaims>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
        }

        public override string Id { get; set; }
        public override string Name { get; set; }
        public override string NormalizedName { get; set; }
        public override string ConcurrencyStamp { get; set; }
        public string Description { get; set; }

        public ICollection<Aspnetroleclaims> Aspnetroleclaims { get; set; }
        public ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
    }
}
