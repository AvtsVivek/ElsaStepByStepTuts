using System;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.Timers.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;
using Elsa.Services.Models;

namespace MxWork.ElsaWf.Tuts.BasicActivities
{
    public class RecurringWorkflow : IWorkflow
    {
        public RecurringWorkflow()
        {

        }
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .WithId("RecurringWorkflow")
                .AsSingleton()
                .StartWith<TimerEvent>(x => x.TimeoutExpression = new LiteralExpression<TimeSpan>("00:00:05"))
                .Then<WriteLine>(x => x.TextExpression = 
                    new JavaScriptExpression<string>("`Trigger received. The time is: ${new Date().toISOString()}`"));
        }
    }
}
