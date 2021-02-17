using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi10Min.DTO
{
    public class LoginDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        // public bool RememberMe { get; set; }
    }
}
