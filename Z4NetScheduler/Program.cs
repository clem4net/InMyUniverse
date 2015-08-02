using System.ServiceProcess;

namespace Z4NetScheduler
{
    public static class Program
    {

        private static void Main()
        {
            ServiceBase.Run(new[] {new Service1()});
        }
    }

}
