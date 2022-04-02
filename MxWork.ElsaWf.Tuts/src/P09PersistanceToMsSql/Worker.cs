using Elsa.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MxWork.ElsaWf.Tuts.BasicActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Extensions;
using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Elsa.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Elsa.Models;

namespace P09PersistanceToMsSql
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IWorkflowRegistry _workflowRegistry;
        //private readonly ElsaContext _elsaContext;
        //private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, 
            IWorkflowRegistry workflowRegistry, 
            //ElsaContext elsaContext,
            //IWorkflowDefinitionStore workflowDefinitionStore,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _logger = logger;
            _workflowRegistry = workflowRegistry;
            //_elsaContext = elsaContext;
            //_workflowDefinitionStore = workflowDefinitionStore;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var workflowDefinitionVersion = await _workflowRegistry.GetWorkflowDefinitionAsync<HelloWorldWorkflow>();
            
            var workflowDefinitionId = nameof(HelloWorldWorkflow);

            workflowDefinitionVersion.IsLatest = true;
            workflowDefinitionVersion.Version = 1;


            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var elsaContext = scope.ServiceProvider.GetRequiredService<ElsaContext>();
                var workflowDefinitionStore = scope.ServiceProvider.GetRequiredService<IWorkflowDefinitionStore>();
                var workflowInvoker = scope.ServiceProvider.GetRequiredService<IWorkflowInvoker>();
                var workflowInstanceStore = scope.ServiceProvider.GetRequiredService<IWorkflowInstanceStore>();

                // When running this program multiple times, 
                // we should delete the created workflow definition before adding it to the store again.
                await workflowDefinitionStore.DeleteAsync(workflowDefinitionId);
                //await elsaContext.SaveChangesAsync();

                await workflowDefinitionStore.SaveAsync(workflowDefinitionVersion);
                //await elsaContext.SaveChangesAsync();

                // Load the workflow definition.
                var workflowDefinitionVersionLoadedFromDb = await workflowDefinitionStore.GetByIdAsync(
                    workflowDefinitionVersion.DefinitionId, VersionOptions.Latest);
                var workflowExecutionContext = await workflowInvoker.StartAsync(workflowDefinitionVersionLoadedFromDb);

                //var workflowInstance = workflowExecutionContext.Workflow.ToInstance();
                // await workflowInstanceStore.SaveAsync(workflowInstance);
            }
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}
