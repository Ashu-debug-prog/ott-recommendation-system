using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService.Infrastructure.Persistence
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();

            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=OttDb;user=root;password=manager;",
                new MySqlServerVersion(new Version(8, 0, 36))
            );

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}