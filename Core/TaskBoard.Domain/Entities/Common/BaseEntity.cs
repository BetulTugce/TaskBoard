﻿using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; }
        virtual public DateTime? UpdatedAt { get; set; }
    }
}
