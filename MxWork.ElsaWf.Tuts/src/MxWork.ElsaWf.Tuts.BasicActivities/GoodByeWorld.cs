using System;
using Elsa.Services;
using Elsa.Results;
using Elsa.Services.Models;

namespace MxWork.ElsaWf.Tuts.BasicActivities
{
    public class GoodByeWorld : Activity
    {
        private static int _counter;
        public GoodByeWorld()
        {
            _counter++;
        }
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            Console.WriteLine("Goodbye cruel world... " + _counter);
            return Done();
        }
    }
}