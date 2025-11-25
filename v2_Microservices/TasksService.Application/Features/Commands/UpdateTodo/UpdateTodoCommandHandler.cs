using AutoMapper;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksService.Application.DTOs;
using TasksService.Domain.Interfaces;

namespace TasksService.Application.Features.Commands.UpdateTodo
{
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, Result<TodoDto>>
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<UpdateTodoCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateTodoCommandHandler(
            ITodoRepository repository,
            ILogger<UpdateTodoCommandHandler> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<TodoDto>> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            // Get Todo from Database
            var todoItem = await _repository.GetTodoByIdAsync(request.UserId, request.Id);

            // Errors Handling
            if (todoItem == null)
            {
                _logger.LogWarning("Update failed. Todo {Id} not found for User {UserId}", request.Id, request.UserId);
                return Result<TodoDto>.Failure("Todo item not found.");
            }

            // Map
            _mapper.Map(request.TodoRequest, todoItem);

            // Save changes
            await _repository.UpdateTodoAsync(todoItem);

            _logger.LogInformation("Todo {Id} updated successfully.", request.Id);

            // Map
            var dto = _mapper.Map<TodoDto>(todoItem);
            return Result<TodoDto>.Success(dto);
        }
    }
}