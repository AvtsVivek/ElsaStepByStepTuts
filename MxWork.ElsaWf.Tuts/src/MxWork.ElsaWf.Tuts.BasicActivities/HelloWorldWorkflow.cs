using Elsa.Services;
using Elsa.Services.Models;

namespace MxWork.ElsaWf.Tuts.BasicActivities
{
    public class HelloWorldWorkflow : IWorkflow
    {
        public HelloWorldWorkflow()
        {

        }
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .StartWith<HelloWorld>()
                .Then<GoodByeWorld>();
        }
    }
}