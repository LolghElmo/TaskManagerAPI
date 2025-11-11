using TaskManagerAPI.Models;

namespace TaskManagerAPI.Interfaces
{
    public interface ITaskRepository
    {
        // Create a new task
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        // Get a task by its ID
        Task<TaskItem?> GetTaskAsync(string user,int id);
        // Get all tasks
        Task<IEnumerable<TaskItem>> GetTasksForUserAsync(string userId);
    }
}
