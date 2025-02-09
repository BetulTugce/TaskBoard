using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Entities.Common;

namespace TaskBoard.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
