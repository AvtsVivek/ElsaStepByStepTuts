using System;
using System.Threading.Tasks;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using MxWork.ElsaWf.Tuts.BasicActivities;

namespace P05WorkflowInstance
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


            var workflowFactory = serviceProvider.GetRequiredService<IWorkflowFactory>();
            var workflow = workflowFactory.CreateWorkflow<HelloWorldWorkflow>();
            var executionContext1 = await workflowInvoker.StartAsync(workflow);

            // Now start the workflow.
            //var executionContext2 = await workflowInvoker.StartAsync<HelloWorldWorkflow>();

            // Now start the workflow.
            var executionContext2 = await workflowInvoker.StartAsync<HelloWorldWorkflow>();
            // Summary.
            // 1. Get the invoker from the service provider. The service provider is a .net core object.
            // 2. Since we have invoker, we can invoke a workflow object.

            var instance2 = executionContext1.Workflow.ToInstance();
            var instance1 = executionContext2.Workflow.ToInstance();
            var instance3 = executionContext1.Workflow.ToInstance();

            Console.WriteLine(instance1.GetHashCode());
            Console.WriteLine(instance2.GetHashCode());
            Console.WriteLine(instance3.GetHashCode());

            Console.ReadLine();
        }
    }
}