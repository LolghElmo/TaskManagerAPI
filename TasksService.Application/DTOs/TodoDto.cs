using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Domain.Models.Enums;

namespace TasksService.Application.DTOs
{
    public class TodoDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = default!;
        public string Priority { get; set; } = default!;
        public DateTimeOffset CreatedDate { get; set; }
    }
}
