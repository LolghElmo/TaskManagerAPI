using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksService.Domain.Interfaces;

namespace TasksService.Application.Features.Commands.DeleteTodo
{
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, Result<bool>>
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<DeleteTodoCommandHandler> _logger;

        public DeleteTodoCommandHandler(
            ITodoRepository repository,
            ILogger<DeleteTodoCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = await _repository.GetTodoByIdAsync(command.UserId, command.Id);

            // Error Handling
            if (todo == null)
            {
                _logger.LogWarning("Delete failed. Todo {Id} not found for User {UserId}", command.Id, command.UserId);
                return Result<bool>.Failure("Todo item not found.");
            }

            // Save & Delete
            await _repository.DeleteTodoAsync(todo);

            _logger.LogInformation("Todo {Id} deleted successfully.", command.Id);

            // Return
            return Result<bool>.Success(true);
        }
    }
}