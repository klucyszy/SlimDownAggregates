using LibraryMembership.Database;
using System.Threading.Tasks;

namespace LibraryMembership.Services;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    public UnitOfWork(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task SaveChangesAsync()
    {
        await _dataContext.SaveChangesAsync();
    }
}