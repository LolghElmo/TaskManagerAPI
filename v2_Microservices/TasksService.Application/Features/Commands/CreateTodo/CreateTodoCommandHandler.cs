using AutoMapper;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksService.Application.DTOs;
using TasksService.Domain.Interfaces; // ✅ Depends on Interface, not Implementation
using TasksService.Domain.Models;

namespace TasksService.Application.Features.Commands.CreateTodo
{
    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Result<TodoDto>>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<CreateTodoCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateTodoCommandHandler(
            ITodoRepository todoRepository,
            ILogger<CreateTodoCommandHandler> logger,
            IMapper mapper)
        {
            _todoRepository = todoRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<TodoDto>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            // Map
            var todoItem = _mapper.Map<TodoItem>(request.TodoRequest);

            todoItem.UserId = request.UserId;

            // Save
            var createdTodo = await _todoRepository.CreateTodoAsync(todoItem);

            if (createdTodo == null)
            {
                _logger.LogError("Failed to create todo for user {UserId}", request.UserId);
                return Result<TodoDto>.Failure("Failed to create todo item.");
            }

            // Map
            var todoDto = _mapper.Map<TodoDto>(createdTodo);

            return Result<TodoDto>.Success(todoDto);
        }
    }
}