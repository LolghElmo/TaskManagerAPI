using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Domain.Models;

namespace TasksService.Domain.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllTodosAsync(string userId);
        Task<TodoItem?> GetTodoByIdAsync(string userId, int todoId);
        Task<TodoItem> CreateTodoAsync(TodoItem todo);
        Task UpdateTodoAsync(TodoItem todo);
        Task DeleteTodoAsync(TodoItem todo);

    }
}
