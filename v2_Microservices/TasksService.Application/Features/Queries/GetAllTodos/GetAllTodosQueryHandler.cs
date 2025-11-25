using AutoMapper;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TasksService.Application.DTOs;
using TasksService.Domain.Interfaces;

namespace TasksService.Application.Features.Queries.GetAllTodos
{
    public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, Result<IEnumerable<TodoDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<GetAllTodosQueryHandler> _logger;
        public GetAllTodosQueryHandler(IMapper mapper, ITodoRepository todoRepository, ILogger<GetAllTodosQueryHandler> logger)
        {
            _mapper = mapper;
            _todoRepository = todoRepository;
            _logger = logger;
        }
        public async Task<Result<IEnumerable<TodoDto>>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            var todos = await _todoRepository.GetAllTodosAsync(request.UserId);

/*            if (!todos.Any())
            {
                _logger.LogWarning("Failed to find. Todos not found for User {UserId}", request.UserId);
                return Result<IEnumerable<TodoDto>>.Failure($"Tasks not found for user {request.UserId}");
            }
*/
            _logger.LogInformation("Retrieved {count} todos successfully.", todos.Count());

            var dto = _mapper.Map<IEnumerable<TodoDto>>(todos);
            return Result<IEnumerable<TodoDto>>.Success(dto);
        }
    }
}
