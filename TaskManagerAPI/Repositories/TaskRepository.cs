using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DataContext _dataContext;

        public TaskRepository(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            // Add the new task to the data context
            await _dataContext.Tasks.AddAsync(task);

            // Save changes to the database and return true if successful
            await _dataContext.SaveChangesAsync();
            return task;
        }

        public Task<bool> DeleteTaskAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskItem> FinishTaskAsync(TaskItem task)
        {
            throw new NotImplementedException();
        }

        public async Task<TaskItem?> GetTaskAsync(string userId,int taskId)
        {
            // Find the task by its ID & UserID
            var task = await _dataContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            return await _dataContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ApplicationUserId == userId);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksForUserAsync(string userId)
        {
            // Retrieve all tasks for the specified user
            var tasks = await _dataContext.Tasks
                .Where(t => t.ApplicationUserId == userId)
                .ToListAsync();
            // Return the list of tasks
            return tasks;
        }

        public Task<TaskItem?> UpdateTaskAsync(TaskItem task)
        {
            throw new NotImplementedException();
        }
    }
}
