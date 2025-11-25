using TasksService.Domain.Models.Enums;

namespace TasksService.Application.DTOs
{
    public class UpdateTodoRequest
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public TodoStatus Status { get; set; }
        public TodoItemPriority Priority { get; set; }
    }
}