using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Domain.Models.Enums;

namespace TasksService.Application.DTOs
{
    public class CreateTodoRequest
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public TodoItemPriority Priority { get; set; } = TodoItemPriority.Medium;
    }
}
