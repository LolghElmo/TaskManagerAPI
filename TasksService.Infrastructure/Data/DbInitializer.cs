using Microsoft.EntityFrameworkCore;

using TasksService.Domain.Interfaces;
using TasksService.Domain.Models;
using TasksService.Domain.Models.Enums;

namespace TasksService.Infrastructure.Data
{
    internal class DbInitializer : IDbInitializer
    {
        private readonly DataContext _dataContext;

        public DbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Initialize()
        {
            try
            {
                if (_dataContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dataContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not apply migrations: {ex.Message}");
            }

            if (!_dataContext.Todos.Any())
            {
                var myUserId = "user-guid-from-identity-service-db";

                var tasks = new List<TodoItem>
            {
                new TodoItem
                {
                    Title = "Learn Microservices",
                    Description = "Complete the course and build the project",
                    UserId = myUserId,
                    Priority = TodoItemPriority.High,
                    Status = TodoStatus.InProgress
                },
                new TodoItem
                {
                    Title = "Setup Docker",
                    Description = "Containerize the SQL Server",
                    UserId = myUserId,
                    Priority = TodoItemPriority.Medium
                }
            };

                _dataContext.Todos.AddRange(tasks);
                _dataContext.SaveChanges();
            }
        }
    }
}
