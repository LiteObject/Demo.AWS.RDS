using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Demo.AWS.RDS
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            /* 
             * ALTER USER 'testuser'@'%' REQUIRE SSL;
             * 
             * $ mysql -h delete-me-database-1.ceiy5yfgzyfi.us-east-1.rds.amazonaws.com
             * -u admin -p 
             * --ssl-ca=[full path]global-bundle.pem 
             * --ssl-mode=VERIFY_IDENTITY 
             */

            /* COPY *.pem file to container's cert folder (create one if it doesn't exist, 
             * $ mkdir /usr/local/share/cert, or use a different folder) 
             * $ docker cp global-bundle.pem mysql-server:/usr/local/share/cert
             * 
             * $ pwd
             * /usr/local/share/cert
             * $ mysql -u admin -p -h delete-me-database-1.ceiy5yfgzyfi.us-east-1.rds.amazonaws.com --ssl-ca=global-bundle.pem --ssl-mode=VERIFY_IDENTITY             
             */

            var connectionuilder = new MySqlConnectionStringBuilder
            {
                Server = "delete-me-database-1.ceiy5yfgzyfi.us-east-1.rds.amazonaws.com",
                Database = "sampledb",
                Port = 3306,
                UserID = "",
                Password = "",
                SslMode = MySqlSslMode.Required,
                TlsVersion = "TLSv1.2",
                SslCa = "global-bundle.pem"
            };

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseMySql(connectionuilder.ConnectionString, ServerVersion.AutoDetect(connectionuilder.ConnectionString));
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.EnableDetailedErrors(true);

            using var context = new MyDbContext(optionsBuilder.Options);
            var can = await context.Database.CanConnectAsync();
            Console.WriteLine(">>> Can connect? " + can);

            Console.WriteLine("\nPress any key to exit.");
            Console.Read();
        }
    }
}