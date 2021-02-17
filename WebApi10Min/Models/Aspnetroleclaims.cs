using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Aspnetroleclaims : IdentityRoleClaim<string>
    {
        public override int Id { get; set; }
        public override string RoleId { get; set; }
        public string Claimtype { get; set; }
        public override string ClaimValue { get; set; }

        public Aspnetroles Role { get; set; }
    }
}
