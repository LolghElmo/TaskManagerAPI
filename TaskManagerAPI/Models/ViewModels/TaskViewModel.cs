using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
