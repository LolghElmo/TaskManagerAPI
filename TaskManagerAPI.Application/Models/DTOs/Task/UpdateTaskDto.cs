
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Application.Models.DTOs.Task
{
    public class UpdateTaskDto
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public bool IsCompleted { get; set; }
    }
}
