using Microsoft.AspNetCore.Identity;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Domain.Entities.Identity;

namespace TaskBoard.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
    }
}
