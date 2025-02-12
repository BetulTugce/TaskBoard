namespace TaskBoard.Application.DTOs.User
{
    public class GetAllUsersResponseDto
    {
        public List<UserResponseDto> Users { get; set; }
        public int TotalUsersCount { get; set; }
    }
}
