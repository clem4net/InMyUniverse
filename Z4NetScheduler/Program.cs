using System.ServiceProcess;

namespace Z4NetScheduler
{
    public static class Program
    {

        private static void Main()
        {
            //ServiceBase.Run(new[] {new Service1()});

            ServiceZDevices.ZDeviceServiceClient cli = new ServiceZDevices.ZDeviceServiceClient();
            var list = cli.ListDevices();
            cli.Close();
        }
    }

}
