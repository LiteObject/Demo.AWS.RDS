using Microsoft.EntityFrameworkCore;

namespace Demo.AWS.RDS
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}
