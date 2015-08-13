using System;
using System.ServiceModel;

namespace Z4Net
{
    /// <summary>
    /// Main program.
    /// </summary>
    public static class Program
    {

        /// <summary>
        /// Start Z service.
        /// </summary>
        public static void Main()
        {
            using (ServiceHost host = new ServiceHost(typeof(ZService)))
            {
                //read the WCF service  
                host.Open();
                Console.WriteLine(DateTime.Now.ToString());
                Console.ReadLine();
                host.Close();

            }
        }

    }
}
