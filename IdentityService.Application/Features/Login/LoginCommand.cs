using Common.Models;
using IdentityService.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityService.Application.Features.Login
{
    public class LoginCommand : IRequest<Result<LoginResponseDto>>
    {
        public required LoginRequestDto LoginRequest { get; set; }
    }
}
