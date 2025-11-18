using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Application.Models.DTOs.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string ApplicationUserId { get; set; } = default!;
    }
}
