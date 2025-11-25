using AutoMapper;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;
using TasksService.Domain.Interfaces;

namespace TasksService.Application.Features.Queries.GetTodo
{
    public class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, Result<TodoDto>>
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTodoQueryHandler> _logger;
        public GetTodoQueryHandler(ITodoRepository repository, IMapper mapper, ILogger<GetTodoQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<TodoDto>> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            var todo = await _repository.GetTodoByIdAsync(request.UserId, request.Id);

            if (todo == null)
            {
                _logger.LogWarning("Failed to find. Todo {Id} not found for User {UserId}", request.Id, request.UserId);
                return Result<TodoDto>.Failure("Task not found");
            }

            _logger.LogInformation("Todo {Id} retrieved successfully.", request.Id);

            var dto = _mapper.Map<TodoDto>(todo);
            return Result<TodoDto>.Success(dto);
        }
    }
}
