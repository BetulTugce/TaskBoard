using TaskBoard.Application.Abstractions.Services.Authentications;

namespace TaskBoard.Application.Abstractions.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
    }
}
