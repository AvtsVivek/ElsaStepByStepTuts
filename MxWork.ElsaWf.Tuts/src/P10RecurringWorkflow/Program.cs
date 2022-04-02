using System.Threading.Tasks;
using Elsa.Activities.Console.Extensions;
using Elsa.Activities.Timers.Extensions;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MxWork.ElsaWf.Tuts.BasicActivities;
using NodaTime;
using System;

namespace P07WorkflowSerialization
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Setup a service collection.
            var serviceProvider = new ServiceCollection()
                .AddElsa()
                .AddConsoleActivities()
                .AddWorkflow<RecurringWorkflow>()
                .AddTimerActivities(options => options.Configure(x => x.SweepInterval = Duration.FromSeconds(1)))
                .BuildServiceProvider()
                ;

            // First get the invoker.
            var workflowInvoker = serviceProvider.GetService<IWorkflowInvoker>();

            var workflowFactory = serviceProvider.GetRequiredService<IWorkflowFactory>();
            var workflow = workflowFactory.CreateWorkflow<RecurringWorkflow>();
            var executionContext1 = await workflowInvoker.StartAsync(workflow);
            
            //// Another way to get execution context is directely from workflow definition class 
            //// as below.
            ////var executionContext2 = await workflowInvoker.StartAsync<HelloWorldWorkflow>();
            

            //var instance = executionContext1.Workflow.ToInstance();

            //var serializer = serviceProvider.GetRequiredService<IWorkflowSerializer>();

            //var jsonFromWorkflowInstance = serializer.Serialize(instance, JsonTokenFormatter.FormatName);

            //var jsonFromWorkflowDef = serializer.Serialize(workflow.Definition, JsonTokenFormatter.FormatName);

            //Console.WriteLine("Json for the workflow instance");

            //Console.WriteLine(jsonFromWorkflowInstance);

            //Console.WriteLine("Json for the workflow definition");

            //Console.WriteLine(jsonFromWorkflowDef);

            //// Summary.
            //// 1. The json that we get from Definition is differnt from Instance.
            //// 2. Instance basically represents an executed instance. While Definition represents 
            //// the actual workflow definition.

            Console.ReadLine();
        }
    }
}