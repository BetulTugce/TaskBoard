﻿using TaskBoard.Application.DTOs.User;

namespace TaskBoard.Application.DTOs.Role
{
    public class RoleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Roleun atandigi kullanicilar
        public List<UserResponseDto> Users { get; set; } = new List<UserResponseDto>();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
