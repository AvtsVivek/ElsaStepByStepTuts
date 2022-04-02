using System;
using Elsa.Services;
using Elsa.Results;
using Elsa.Services.Models;

namespace MxWork.ElsaWf.Tuts.BasicActivities
{
    public class HelloWorld : Activity
    {
        private static int _counter;
        public HelloWorld()
        {
            _counter++;
        }
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            Console.WriteLine("Hello world! " + _counter);
            return Done();
        }
    }
}