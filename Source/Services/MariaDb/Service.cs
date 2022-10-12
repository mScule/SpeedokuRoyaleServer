using SpeedokuRoyaleServer.Models.DbContexts;

namespace SpeedokuRoyaleServer.Models.Services.MariaDB;

public abstract class Service
{
    protected MariaDbContext dbContext;
    public Service(MariaDbContext dbContext) =>
        this.dbContext = dbContext;
}
