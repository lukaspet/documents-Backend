using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Aspnetuserlogins : IdentityUserLogin<string>
    {
        public override string LoginProvider { get; set; }
        public override string ProviderKey { get; set; }
        public override string ProviderDisplayName { get; set; }
        public override string UserId { get; set; }

        public Aspnetusers User { get; set; }
    }
}
