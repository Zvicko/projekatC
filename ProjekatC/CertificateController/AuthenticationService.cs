using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace CertificateController
{
    public class AuthenticationService
    {
        // Mapirani javni kljucevi i korisnicka imena validnih klijenata.
        private Dictionary<string, string> validClients = new Dictionary<string, string>();

        public AuthenticationService() { }

        /// <summary>
        /// Vrsi autentifikaciju klijenta.
        /// </summary>
        /// <param name="username"> Korisnicko ime klijenta. </param>
        /// <param name="certificate"> Klijentski sertifikat. </param>
        public bool Authenticate(string username, X509Certificate2 certificate)
        {
            bool authenticated = false;

            CertificateValidator cv = new CertificateValidator();

            cv.Validate(certificate);

            if (cv.CertificateValid == true)
            {
                validClients.Add(certificate.PublicKey.Oid.Value, username);
                authenticated = true;
            }

            return authenticated;
        }

        public Dictionary<string, string> ValidClients
        {
            get { return validClients; }
            set { validClients = value; }
        }
    }
}
