using AutoMapper;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksService.Application.DTOs;
using TasksService.Domain.Interfaces;

namespace TasksService.Application.Features.Queries.GetAllTodos
{
    public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, Result<PagedResult<TodoDto>>>
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

        public async Task<Result<PagedResult<TodoDto>>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _todoRepository.GetAllTodosAsync(
                request.UserId,
                request.Page,
                request.PageSize);

            _logger.LogInformation("Retrieved {Count} todos (page {Page} of {TotalPages}) for user {UserId}.",
                items.Count(), request.Page, (int)Math.Ceiling((double)totalCount / request.PageSize), request.UserId);

            var pagedResult = new PagedResult<TodoDto>
            {
                Items = _mapper.Map<IEnumerable<TodoDto>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return Result<PagedResult<TodoDto>>.Success(pagedResult);
        }
    }
}
