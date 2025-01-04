using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskBoard.Persistence.Contexts;

namespace TaskBoard.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TaskBoardDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));
        }
    }
}
