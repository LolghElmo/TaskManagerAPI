using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Domain.Models.Enums;

namespace TasksService.Domain.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string UserId { get; set; }
        public string? Description { get; set; }
        public TodoStatus Status { get; set; } = TodoStatus.Pending;
        public TodoItemPriority Priority { get; set; } = TodoItemPriority.Low;
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DueDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
