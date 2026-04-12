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
                if (_dataContext.Database.GetPendingMigrations().Any())
                {
                    _dataContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not apply migrations: {ex.Message}");
                return;
            }
        }
    }
}
