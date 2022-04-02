using System;
using System.Threading.Tasks;
using Elsa;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.Console.Extensions;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Serialization;
using Elsa.Serialization.Formatters;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using MxWork.ElsaWf.Tuts.BasicActivities;

namespace P01WorkflowDefinitionVersion
{
    // This introduces the type WorkflowDefinitionVersion
    class Program
    {
        private static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddElsa()
                .AddConsoleActivities()
                .BuildServiceProvider();

            // Define a workflow as data so we can store it somewhere (file, database, etc.).
            var workflowDefinitionVersion = new WorkflowDefinitionVersion
            {
                Activities = new[]
                {
                    new ActivityDefinition<WriteLine>("activity-1", new { TextExpression = new LiteralExpression("Hello world!")}),
                    new ActivityDefinition<WriteLine>("activity-2", new { TextExpression = new LiteralExpression("Goodbye world...")})
                },
                Connections = new[]
                {
                    new ConnectionDefinition("activity-1", "activity-2", OutcomeNames.Done),
                }
            };

            // Run the workflow.
            var workflowInvoker = serviceProvider.GetService<IWorkflowInvoker>();
            var executionContext = await workflowInvoker.StartAsync(workflowDefinitionVersion);

            Console.ReadLine();
        }
    }

}