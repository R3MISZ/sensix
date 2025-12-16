using Sensix.Lib.Database;

namespace Sensix.Lib.Repository;

public interface IDbRepository
{
    Task<int> SaveChangesAsync();
}

public class DbRepository : IDbRepository
{
    private readonly SensixDbContext _db;
    public DbRepository(SensixDbContext db) => _db = db;

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
