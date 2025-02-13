using Microsoft.Extensions.DependencyInjection;
using TaskBoard.Application.Abstractions.Token;
using TaskBoard.Infrastructure.Services.Token;

namespace TaskBoard.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        }
    }
}
