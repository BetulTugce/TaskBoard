using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskBoard.Application.Abstractions.Services;
using TaskBoard.Application.Repositories;
using TaskBoard.Persistence.Contexts;
using TaskBoard.Persistence.Repositories;
using TaskBoard.Persistence.Repositories.Task;
using TaskBoard.Persistence.Services;

namespace TaskBoard.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TaskBoardDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

            services.AddScoped<ITaskReadRepository, TaskReadRepository>();
            services.AddScoped<ITaskWriteRepository, TaskWriteRepository>();

            services.AddScoped<ITeamMemberReadRepository, TeamMemberReadRepository>();
            services.AddScoped<ITeamMemberWriteRepository, TeamMemberWriteRepository>();

            services.AddScoped<ITeamReadRepository, TeamReadRepository>();
            services.AddScoped<ITeamWriteRepository, TeamWriteRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITeamService, TeamService>();
        }
    }
}
