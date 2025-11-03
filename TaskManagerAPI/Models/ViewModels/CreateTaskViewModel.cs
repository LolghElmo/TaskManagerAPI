using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models.ViewModels
{
    public class CreateTaskViewModel
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
