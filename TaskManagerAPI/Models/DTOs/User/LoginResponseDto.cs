namespace TaskManagerAPI.Models.DTOs.User
{
    public class LoginResponseDto
    {
        public string Message { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
