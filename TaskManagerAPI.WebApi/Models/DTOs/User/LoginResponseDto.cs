namespace TaskManagerAPI.WebApi.Models.DTOs.User
{
    public class LoginResponseDto
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
