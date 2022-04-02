using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Elsa.Activities.Console.Extensions;
using Elsa.Models;
using Elsa.Serialization;
using Elsa.Serialization.Formatters;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using MxWork.ElsaWf.Tuts.BasicActivities;

namespace P08WorkflowDeSerialization
{
    internal class Program
    {
        // This example shows deserialization. 
        private static async Task Main()
        {
            var namespaceName = typeof(Program).Namespace;
            var simpleWorkflowDefinitionJson = await ReadEmbeddedResourceAsync(namespaceName + ".simpleWorkflowDefinition.json");
            var serviceProvider = BuildServices();
            var serializer = serviceProvider.GetRequiredService<IWorkflowSerializer>();
            var workflow = serializer.Deserialize<WorkflowDefinitionVersion>(simpleWorkflowDefinitionJson, JsonTokenFormatter.FormatName);
            var workflowInvoker = serviceProvider.GetRequiredService<IWorkflowInvoker>();
            
            await workflowInvoker.StartAsync(workflow);

            var simpleWorkflowInstanceJson = await ReadEmbeddedResourceAsync(namespaceName + ".simpleWorkflowInstance.json");
            var workflowInstance = serializer.Deserialize<WorkflowInstance>(simpleWorkflowInstanceJson, JsonTokenFormatter.FormatName);
            Console.ReadLine();
        }

        private static IServiceProvider BuildServices()
        {

            var serviceProvider = new ServiceCollection()
                .AddElsa()
                .AddActivity<HelloWorld>()
                .AddActivity<GoodByeWorld>()
                .BuildServiceProvider()
                ;
            return serviceProvider;
        }

        private static async Task<string> ReadEmbeddedResourceAsync(string resourceName)
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            using var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName));
            return await reader.ReadToEndAsync();
        }
    }


}