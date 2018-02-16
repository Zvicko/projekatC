using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
namespace CertificateController
{
    public class TicketGrantingService
    {
        private List<string> dnsTable;
        private Dictionary<string, string> startedServices;
        private bool check = false;
        public TicketGrantingService()
        {
            dnsTable = new List<string>();
            startedServices = new Dictionary<string, string>();
        }

        public void CheckServiceInDomain(string dns,string idService)
        {

            if (dnsTable.Contains(dns))
            {
                if (startedServices.ContainsKey(dns))
                {
                    if (startedServices[dns].Contains(idService))
                    {
                        check = false;
                    }
                }
                check = true;
                startedServices.Add(dns, idService);
            }
            else
            {
                check = false;
            }
          
        }
        /*
        public AsymmetricAlgorithm SendPublicKey()
        {
            if(check)
            {


            }
        }
        */


    }
}
