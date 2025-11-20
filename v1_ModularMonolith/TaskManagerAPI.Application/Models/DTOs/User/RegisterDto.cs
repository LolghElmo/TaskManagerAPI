using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Application.Models.DTOs.User
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage ="The {0} must be at least {2} characters long.", MinimumLength =6)]
        public string Password { get; set; }
        [Required]
        [Display(Name="Confirm Password")]
        [Compare("Password", ErrorMessage ="The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Role {  get; set; }
    }
}
