using System;
using System.Threading.Tasks;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using MxWork.ElsaWf.Tuts.BasicActivities;

namespace P04InvokeWorkFlowFromWorkflowObj
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


            Console.ReadLine();
        }
    }
}