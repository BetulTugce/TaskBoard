namespace TaskBoard.Application.DTOs.User
{
    public class LoginUserRequestDto
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
