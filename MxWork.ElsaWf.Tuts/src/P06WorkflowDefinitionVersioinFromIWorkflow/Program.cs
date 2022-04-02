using System;
using System.Threading.Tasks;
using Elsa.Serialization;
using Elsa.Serialization.Formatters;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using MxWork.ElsaWf.Tuts.BasicActivities;
using System.Data;
using Elsa.Activities.Console.Extensions;
using Elsa.Extensions;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Runtime;

namespace P06WorkflowDefinitionVersioinFromIWorkflow
{
    class Program
    {
        // This example demonistrates how to get workflow definition versioin object from 
        // IWorkflow type.
        static async Task Main(string[] args)
        {
            // Setup a service collection.
            var serviceProvider = new ServiceCollection()
                .AddElsa()
                .AddActivity<HelloWorld>()
                .AddActivity<GoodByeWorld>()
                .AddWorkflow<HelloWorldWorkflow>()
                .BuildServiceProvider()
                ;

            var registry = serviceProvider.GetService<IWorkflowRegistry>();
            var workflowDefinitionVersion = await registry.GetWorkflowDefinitionAsync<HelloWorldWorkflow>();

            // Once we have workflow definition version, we can invode it, or serialize it.
            var workflowInvoker = serviceProvider.GetService<IWorkflowInvoker>();
            var executionContext = await workflowInvoker.StartAsync(workflowDefinitionVersion);

            Console.ReadLine();
        }
    }
}