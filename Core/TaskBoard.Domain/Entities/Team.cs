﻿using System.ComponentModel.DataAnnotations.Schema;
using TaskBoard.Domain.Entities.Common;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Domain.Entities
{
    public class Team : BaseEntity
    {
        public required string Name { get; set; }

        // Takimin yoneticisi (bir kullanici)
        [ForeignKey("Manager")]
        public Guid? ManagerId { get; set; }
        public virtual ApplicationUser Manager { get; set; }

        // Takima ait uyeler
        public virtual ICollection<TeamMember> TeamMembers { get; set; }

        // Takima ait gorevler
        public virtual ICollection<Task> Tasks { get; set; }
    }
}