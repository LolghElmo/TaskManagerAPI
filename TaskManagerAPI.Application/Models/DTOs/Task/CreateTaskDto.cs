using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Application.Models.DTOs.Task
{
    public class CreateTaskDto
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
