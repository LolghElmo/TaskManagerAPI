using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdenttiyService.Domain.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; } = string.Empty;
    }
}
