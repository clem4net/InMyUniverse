using System.ServiceProcess;
using Z4NetScheduler.Business;

namespace Z4NetScheduler
{
    public static class Program
    {

        private static void Main()
        {
            //ServiceBase.Run(new[] {new Service1()});

            TaskBusiness.Process();

        }
    }

}
