using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:5500/DataManagment";
            using (ClientProxy clp = new ClientProxy(binding, address))
            {
                clp.Read();
                Console.WriteLine("read...");
                clp.Write();
                Console.WriteLine("write...");
                clp.Close();
                Console.WriteLine("close...");
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
