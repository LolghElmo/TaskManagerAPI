using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerAPI.Application.Models.DTOs.Task;
using TaskManagerAPI.Core.Interfaces;

namespace TaskManagerAPI.Application.Features.Tasks
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public string? UserId { get; set; }
        public int TaskId { get; set; }
    }

    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTaskByIdQueryHandler> _logger;
        public GetTaskByIdQueryHandler(ITaskRepository taskRepository, IMapper mapper, ILogger<GetTaskByIdQueryHandler> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch the task from the repository
            var task = await _taskRepository.GetTaskAsync(request.UserId, request.TaskId);

            // Log the fetching of the task
            _logger.LogInformation("Fetched task with ID {TaskId} for user {UserId}", request.TaskId, request.UserId);
            return _mapper.Map<TaskDto>(task);
        }
    }
}
