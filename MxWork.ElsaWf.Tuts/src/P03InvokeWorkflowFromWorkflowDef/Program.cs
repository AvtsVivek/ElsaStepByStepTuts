using System;
using System.Threading.Tasks;
using Elsa.Serialization;
using Elsa.Serialization.Formatters;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using MxWork.ElsaWf.Tuts.BasicActivities;

namespace P03InvokeWorkflowFromWorkflowDef
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup a service collection.
            var serviceProvider = new ServiceCollection()
                .AddElsa()
                .AddActivity<HelloWorld>()
                .AddActivity<GoodByeWorld>()
                .BuildServiceProvider()
                ;

            // First get the invoker.
            var workflowInvoker = serviceProvider.GetService<IWorkflowInvoker>();

            // Now start the workflow.
            var executionContext = await workflowInvoker.StartAsync<HelloWorldWorkflow>();
            // Summary.
            // 1. Get the invoker from the service provider. The service provider is a .net core object.
            // 2. Since we have invoker, we can invoke a workflow object.

            // The following 4 lines demonistrate serialization.
            // Serialization is delt seperately in an another example. 
            var instance = executionContext.Workflow.ToInstance();

            var serializer = serviceProvider.GetRequiredService<IWorkflowSerializer>();

            var json1 = serializer.Serialize(instance, JsonTokenFormatter.FormatName);

            Console.Write(json1);

            Console.ReadLine();
        }
    }
}