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
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
        public string? UserId { get; set; }
        public int TaskId { get; set; }
    }

    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTasksQueryHandler> _logger;
        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper, ILogger<GetAllTasksQueryHandler> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            // Retrieve tasks from the repository
            var tasks = await _taskRepository.GetTaskAsync(request.UserId, request.TaskId);

            // Map the tasks to TaskDto
            var taskDtos = _mapper.Map<IEnumerable<TaskDto>>(tasks);
            _logger.LogInformation("Retrieved {Count} tasks for UserId: {UserId}", taskDtos.Count(), request.UserId);

            // Return the list of TaskDto
            return taskDtos;
        }
    }   
}
