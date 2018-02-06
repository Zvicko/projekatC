using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:5500/DataManagment";

            ServiceHost host = new ServiceHost(typeof(DataManagment));
            host.AddServiceEndpoint(typeof(IDataManagment), binding, address);

            try
            {
                host.Open();

                Console.WriteLine("Server has been started.\n");
                Console.WriteLine("Press any key to stop the server...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error :" + e.ToString());
            }
            finally
            {
                host.Close();
            }
        }
    }
}
