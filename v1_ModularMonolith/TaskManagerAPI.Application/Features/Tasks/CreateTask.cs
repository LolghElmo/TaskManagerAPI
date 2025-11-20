using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerAPI.Application.Models.DTOs.Task;
using TaskManagerAPI.Core.Interfaces;
using TaskManagerAPI.Core.Models;

namespace TaskManagerAPI.Application.Features.Tasks
{
    // Command to create a new task
    public record CreateTaskCommand(string UserId,
                                    string Name,
                                    string? Description,
                                    DateTime DueDate) : IRequest<TaskDto>;

    // Handler for creating a new task
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTaskCommandHandler> _logger;
        public CreateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper, ILogger<CreateTaskCommandHandler> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            // Map the command to TaskItem entity

            var task = _mapper.Map<TaskItem>(request);
            task.CreatedDate = DateTime.UtcNow;
            task.ApplicationUserId = request.UserId;
            task.IsCompleted = false;

            // Create the task in the repository

            var created = await _taskRepository.CreateTaskAsync(task);
            _logger.LogInformation("Task created with ID: {TaskId} by User ID: {UserId}", created.Id, created.ApplicationUserId);

            // Map the created task to TaskDto and return
            return _mapper.Map<TaskDto>(created);
        }
    }
}
