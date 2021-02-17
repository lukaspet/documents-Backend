using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Aspnetusertokens : IdentityUserToken<string>
    {
        public override string UserId { get; set; }
        public override string LoginProvider { get; set; }
        public override string Name { get; set; }
        public override string Value { get; set; }

        public Aspnetusers User { get; set; }
    }
}
