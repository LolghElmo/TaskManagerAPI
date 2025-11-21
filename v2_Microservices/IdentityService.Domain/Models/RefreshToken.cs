using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IdentityService.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public required string UserId { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation property
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}
