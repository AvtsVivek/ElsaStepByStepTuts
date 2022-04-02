using System;
using System.Threading.Tasks;
using Elsa.Serialization;
using Elsa.Serialization.Formatters;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using MxWork.ElsaWf.Tuts.BasicActivities;

namespace P07WorkflowSerialization
{
    class Program
    {
        // In this example we are serializing a workflow to get the json.
        // There are two types of objects that we try to serialize. 
        // The workflow definition and the the other one is workflow instance.
        // Workflow definition means WorkflowDefinitionVersion. We serialize this object and that can be deserialized back to this.
        // For serializing an instance, first we have to get the Workflow object. 
        // Then we have to ge the WorkflowInstance object.
        // Then we serialize that and then deserialize that as well. See the deserialization example for deserialization
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
            
            // Another way to get execution context is directely from workflow definition class 
            // as below.
            //var executionContext2 = await workflowInvoker.StartAsync<HelloWorldWorkflow>();
            

            var instance = executionContext1.Workflow.ToInstance();

            var serializer = serviceProvider.GetRequiredService<IWorkflowSerializer>();

            var jsonFromWorkflowInstance = serializer.Serialize(instance, JsonTokenFormatter.FormatName);

            var jsonFromWorkflowDef = serializer.Serialize(workflow.Definition, JsonTokenFormatter.FormatName);

            Console.WriteLine("Json for the workflow instance");

            Console.WriteLine(jsonFromWorkflowInstance);

            Console.WriteLine("Json for the workflow definition");

            Console.WriteLine(jsonFromWorkflowDef);

            // Summary.
            // 1. The json that we get from Definition is differnt from Instance.
            // 2. Instance basically represents an executed instance. While Definition represents 
            // the actual workflow definition.

            Console.ReadLine();
        }
    }
}