using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Common;
using CertificateController;
using System.Security.Principal;

namespace Client
{
    public class ClientProxy : ChannelFactory<IDataManagment>, IDataManagment, IDisposable
    {
        IDataManagment factory;
       
        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            string clientCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 clientCert = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientCertCN);

            this.Credentials.ClientCertificate.Certificate = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientCertCN);

            AuthenticationService authService = new AuthenticationService();

            if (authService.Authenticate("Client", clientCert))
            {
                factory = this.CreateChannel();
            }
        }

        public void Write(byte[] id, byte[] name)
        {
            factory.Write(id, name);
        }

        public Dictionary<byte[], byte[]> Read()
        {
            return factory.Read();   
        }
    }
}
