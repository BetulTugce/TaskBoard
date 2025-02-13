namespace TaskBoard.Application.DTOs.Role
{
    public class UpdateRoleRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
