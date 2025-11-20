
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Application.Models.DTOs.Task
{
    public class UpdateTaskDto
    {
        [Required]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
