using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebApi10Min.Models
{
    public partial class Aspnetusers : IdentityUser
    {
        public Aspnetusers()
        {
            Aspnetuserclaims = new HashSet<Aspnetuserclaims>();
            Aspnetuserlogins = new HashSet<Aspnetuserlogins>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
            Aspnetusertokens = new HashSet<Aspnetusertokens>();
        }

        public override string Id { get; set; }
        public string FullName { get; set; }
        public override string UserName { get; set; }
        public override string NormalizedUserName { get; set; }
        public override string Email { get; set; }
        public override string NormalizedEmail { get; set; }
        public override bool EmailConfirmed { get; set; }
        public override string PasswordHash { get; set; }
        public override string SecurityStamp { get; set; }
        public override string ConcurrencyStamp { get; set; }
        public override string PhoneNumber { get; set; }
        public override bool PhoneNumberConfirmed { get; set; }
        public override bool TwoFactorEnabled { get; set; }
        public override DateTimeOffset? LockoutEnd { get; set; }
        public override bool LockoutEnabled { get; set; }
        public override int AccessFailedCount { get; set; }

        public ICollection<Aspnetuserclaims> Aspnetuserclaims { get; set; }
        public ICollection<Aspnetuserlogins> Aspnetuserlogins { get; set; }
        public ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
        public ICollection<Aspnetusertokens> Aspnetusertokens { get; set; }
    }
}
