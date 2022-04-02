using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P09PersistanceToMsSql
{
    public class SqlServerContextFactory : IDesignTimeDbContextFactory<SqlServerContext>
    {
        public SqlServerContext CreateDbContext(string[] args)
        {

            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{ currentEnv ?? "Production"}.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("MsSqlLocalDbConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<SqlServerContext>();
            //var migrationAssembly = typeof(SqlServerContext).Assembly.FullName;
            var migrationAssembly = this.GetType().Assembly.FullName;

            if (connectionString == null)
                throw new InvalidOperationException("Set the EF_CONNECTIONSTRING environment variable to a valid SQL Server connection string. E.g. SET EF_CONNECTIONSTRING=Server=localhost;Database=Elsa;User=sa;Password=Secret_password123!;");

            optionsBuilder.UseSqlServer(
                connectionString,
                x => x.MigrationsAssembly(migrationAssembly)
            );

            return new SqlServerContext(optionsBuilder.Options);
        }
    }

}
