using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Aspnetuserroles : IdentityUserRole<string>
    {
        public override string UserId { get; set; }
        public override string RoleId { get; set; }

        public Aspnetroles Role { get; set; }
        public Aspnetusers User { get; set; }
    }
}
