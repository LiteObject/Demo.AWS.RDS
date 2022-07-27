namespace Demo.AWS.RDS
{
    using Microsoft.EntityFrameworkCore;
    using MySql.Data.MySqlClient;

    public static class Program
    {
        static async Task Main(string[] args)
        {
            await CheckDbConnWithAdoAsync();

            Console.WriteLine("\nProgram ended. Press any key to exit.");
            Console.Read();
        }

        private static async Task CheckDbConnWithDbContextAsync()
        {
            string connStr = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.EnableDetailedErrors(true);

            using var context = new MyDbContext(optionsBuilder.Options);
            var can = await context.Database.CanConnectAsync();
            Console.WriteLine(">>> Can connect? " + can);

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SHOW SESSION STATUS;";
                await context.Database.OpenConnectionAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read() && reader.HasRows)
                    {
                        Console.WriteLine(reader.GetFieldValue<string>(0) + ": " + reader.GetFieldValue<string>(1));
                    }
                }
            }
        }

        private static async Task CheckDbConnWithAdoAsync()
        {
            string connStr = GetConnectionString();
            MySqlConnection conn = new(connStr);
            Console.WriteLine(">>> Connection string: " + conn.ConnectionString);

            try
            {
                Console.WriteLine(">>> Attempting to connect to database");

                await conn.OpenAsync();

                Console.WriteLine(">>> Connection to Database Successful");

                using (MySqlCommand command = new("SHOW SESSION STATUS LIKE \'%ssl%\';", conn))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read() && reader.HasRows)
                        {
                            Console.WriteLine(reader.GetFieldValue<string>(0) + ": " + reader.GetFieldValue<string>(1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString(), ex);
            }
            finally
            {
                Console.WriteLine(">>> Attempting to close database connection");
                await conn.CloseAsync();
            }
        }

        private static string GetConnectionString()
        {
            /*
             * The path to a CA certificate file in a PEM Encoded (.pem) format. This should be used with SslMode=VerifyCA or 
             * SslMode=VerifyFull to enable verification of a CA certificate that is not trusted by the operating system’s certificate store.   
             * 
             * VerifyCA - Always use SSL. Validates the CA but tolerates hostname mismatch.
             * VerifyFull - Always use SSL. Validates CA and hostname.
             * Required - Always use SSL. Deny connection if server does not support SSL. Does not validate CA or hostname.
             * Preferred - (this is the default). Use SSL if the server supports it.
             * None - Do not use SSL.
             */

            return new MySqlConnectionStringBuilder
            {
                Server = "***.us-east-1.rds.amazonaws.com",
                Database = "sampledb", // CREATE DATABASE sampledb;
                Port = 3306,
                UserID = "admin",
                Password = "",
                SslMode = MySqlSslMode.Required,
                TlsVersion = "TLSv1.2"
            }.ConnectionString;
        }
    }
}