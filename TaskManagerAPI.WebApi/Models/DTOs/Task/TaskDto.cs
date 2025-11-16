using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.WebApi.Models.DTOs.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
