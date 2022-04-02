using System;
using System.Threading.Tasks;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.Console.Extensions;
using Elsa.Expressions;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;

namespace P02WorkflowBuilder
{
    // This introduces the type Workflow builder.
    class Program
    {
        private static async Task Main(string[] args)
        {
            // Setup a service collection.
            var serviceProvider = new ServiceCollection()
                .AddElsa()
                .AddConsoleActivities()
                .BuildServiceProvider();

            // Define a workflow.
            var workflowBuilder = serviceProvider.GetRequiredService<IWorkflowBuilder>();

            // You can get the build in a bit differenet way as well.
            // https://dev.to/shimmer/how-to-avoid-the-factory-pattern-in-c-1i6k
            // https://christopherjmcclellan.wordpress.com/2015/09/20/replacing-simple-factories-with-functresult-delegate/
            var workflowBuilderFactory = serviceProvider.GetRequiredService<Func<IWorkflowBuilder>>();
            var workflowBuilder1 = workflowBuilderFactory();

            // By any way, if we have the builder, then we can create a workflow definition version.

            var workflowDefinitionVersion = workflowBuilder
                .StartWith<WriteLine>(x => x.TextExpression = new LiteralExpression("Hello world!"))
                .Then<WriteLine>(x => x.TextExpression = new LiteralExpression("Goodbye and ThankYou world..."))
                .Build()
                ;

            // Start the workflow.
            var workflowInvoker = serviceProvider.GetService<IWorkflowInvoker>();
            await workflowInvoker.StartAsync(workflowDefinitionVersion);

            Console.ReadLine();
        }
    }
}