using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TaskManagerAPI.Core.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]                
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime DueDate { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = default!;
    }

}
