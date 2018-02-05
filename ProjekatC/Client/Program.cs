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
            List<string> list = new List<string>();
            string option = string.Empty;
            using (ClientProxy clp = new ClientProxy(binding, address))
            {
               
                
                while (true)
                {
                    Console.WriteLine("--- MENU ---");
                    Console.WriteLine("1.Read");
                    Console.WriteLine("2.Write");
                    Console.WriteLine("3.Close");
                    Console.WriteLine("Press number and enter button to execute command:");
                    option = Console.ReadLine().Trim();

                    if (option == "1")
                    {
                        list = clp.Read();
                        list.ForEach(o => Console.WriteLine(o));
                       
                        
                    }
                    else if (option == "2")
                    {
                        clp.Write();
                        Console.WriteLine("write...");

                    }
                    else if (option == "3")
                    {
                        clp.Close();
                        break;
                    }



                }
                
               
                
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
