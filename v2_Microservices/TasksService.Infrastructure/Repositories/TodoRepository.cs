using Microsoft.EntityFrameworkCore; // CRITICAL: Needed for ToListAsync, FirstOrDefaultAsync
using TasksService.Domain.Interfaces;
using TasksService.Domain.Models;
using TasksService.Infrastructure.Data;

namespace TasksService.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;

        public TodoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> CreateTodoAsync(TodoItem todo)
        {
            await _context.Todos.AddAsync(todo);

            await _context.SaveChangesAsync();

            return todo;
        }

        public async Task DeleteTodoAsync(TodoItem todo)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodosAsync(string userId)
        {
            return await _context.Todos
                .Where(x => x.UserId == userId) 
                .OrderByDescending(x => x.CreatedDate) 
                .ToListAsync(); 
        }

        public async Task<TodoItem?> GetTodoByIdAsync(string userId, int todoId)
        {
            return await _context.Todos
                .FirstOrDefaultAsync(x => x.Id == todoId && x.UserId == userId);
        }

        public async Task UpdateTodoAsync(TodoItem todo)
        {
            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();
        }
    }
}