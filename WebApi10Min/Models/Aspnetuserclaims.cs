﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Aspnetuserclaims : IdentityUserClaim<string>
    {
        public override int Id { get; set; }
        public override string UserId { get; set; }
        public override string ClaimType { get; set; }
        public override string ClaimValue { get; set; }

        public Aspnetusers User { get; set; }
    }
}
