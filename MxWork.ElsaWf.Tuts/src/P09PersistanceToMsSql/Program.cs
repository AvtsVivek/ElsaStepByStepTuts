using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


using Elsa.Activities.Email.Extensions;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Timers.Extensions;
//using Elsa.Dashboard.Extensions;
using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Elsa.Persistence.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using MxWork.ElsaWf.Tuts.BasicActivities;

namespace P09PersistanceToMsSql
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                //.ConfigureLogging(logging => logging.AddConsole())
                .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var migrationAssembly = typeof(SqlServerContext).Assembly.FullName;
                    var msSqlServerConnectionString = configuration
                            .GetConnectionString("MsSqlLocalDbConnectionString");

                    services.AddElsa(
                        elsaBuilder => elsaBuilder.AddEntityFrameworkStores<SqlServerContext>(
                            dbContextOptionsBuilder =>
                            {
                                dbContextOptionsBuilder.UseSqlServer(msSqlServerConnectionString,
                                    b => b.MigrationsAssembly(migrationAssembly));
                            }))
                    .AddActivity<HelloWorld>()
                    .AddActivity<GoodByeWorld>()
                    .AddWorkflow<HelloWorldWorkflow>()
                    .AddHostedService<Worker>();
                });
    }
}
