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
        // Finish Task
        Task<TaskItem> FinishTaskAsync(TaskItem task);
        // Get Task By TaskID (ADMIN)
        Task<TaskItem?> GetTaskByIdAdminAsync(int taskId);
        // Delete Tasks (ADMIN)
        Task<bool> DeleteTaskAsync(TaskItem task);
        // Update Task (ADMIN)
        Task<TaskItem?> UpdateTaskAsync(TaskItem task);

    }
}
