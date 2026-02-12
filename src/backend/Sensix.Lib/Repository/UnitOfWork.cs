using Sensix.Lib.Database;

namespace Sensix.Lib.Repository;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db; 
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
