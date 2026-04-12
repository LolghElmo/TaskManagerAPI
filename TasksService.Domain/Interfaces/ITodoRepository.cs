using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Domain.Models;

namespace TasksService.Domain.Interfaces
{
    public interface ITodoRepository
    {
        Task<(IEnumerable<TodoItem> Items, int TotalCount)> GetAllTodosAsync(string userId, int page, int pageSize);
        Task<TodoItem?> GetTodoByIdAsync(string userId, int todoId);
        Task<TodoItem> CreateTodoAsync(TodoItem todo);
        Task UpdateTodoAsync(TodoItem todo);
        Task DeleteTodoAsync(TodoItem todo);

    }
}
