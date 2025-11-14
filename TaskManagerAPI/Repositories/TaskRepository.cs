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
        #region USERS CRUDS METHODS
        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            // Add the new task to the data context
            await _dataContext.Tasks.AddAsync(task);

            // Save changes to the database and return true if successful
            await _dataContext.SaveChangesAsync();
            return task;
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
        public async Task<TaskItem> FinishTaskAsync(TaskItem task)
        {
            // Mark the task as completed
            task.IsCompleted = true;
            // Update the task in the data context
            _dataContext.Tasks.Update(task);
            await _dataContext.SaveChangesAsync();
            return task;
        }
        #endregion

        #region ADMIN CRUDS METHODS

        public async Task<TaskItem?> GetTaskByIdAdminAsync(int id)
        {
            // Find the task by its ID
            return await _dataContext.Tasks.Where(t => t.Id == id).FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteTaskAsync(TaskItem task)
        {
            // Remove the task from the data context
            _dataContext.Tasks.Remove(task);
            // Save changes to the database and return true if successful
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<TaskItem?> UpdateTaskAsync(TaskItem task)
        {
            // Update the task in the data context
            _dataContext.Tasks.Update(task);
            await _dataContext.SaveChangesAsync();
            return task;
        }
        #endregion
    }
}
