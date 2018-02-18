using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.IdentityModel;
using System.Security.Cryptography.X509Certificates;
using CertificateController;
using Common;
using System.Security.Principal;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:5500/DataManagment";

            ServiceHost host = new ServiceHost(typeof(DataManagment));
            host.AddServiceEndpoint(typeof(IDataManagment), binding, address);

            //host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            //host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            TicketGrantingService ticketGrantingService = new TicketGrantingService();
            Logger logger = new Logger();

            string serverCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 serverCert = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, serverCertCN);

            if (serverCert != null)
            {
                try
                {
                    host.Open();

                    Console.WriteLine("Server has been successfully started.\n");
                    Console.WriteLine("Press ENTER key to stop the server...");
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
            else
            {
                Console.WriteLine("Server certificate not provided. Server can not be started.\n");
                Console.WriteLine("Press ENTER key to exit...");
                Console.ReadLine();
            }
        }
    }
}