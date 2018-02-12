using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Diagnostics;

namespace CertificateController
{
    public class CertificateValidator : X509CertificateValidator
    {
        private bool certificateValid;

        public CertificateValidator()
        {
            certificateValid = false;
        }

        /// <summary>
        /// Funkcija za validaciju sertifikata sa klijenske strane.
        /// </summary>
        /// <param name="certificate"> Sertifikat koji je potrebno validirati. </param>
        public override void Validate(X509Certificate2 certificate)
        {
            X509Certificate2 cert = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            if (certificate.Issuer != cert.Issuer)
            {
                Logger.AnnotateEvent("Authentication failed. Certificate is not self signed.\n");

                throw new Exception("Certificate is not self signed.\n");
            }
            else
            {
                certificateValid = true;
            }
        }

        public bool CertificateValid
        {
            get { return certificateValid; }
            set { certificateValid = value; }
        }
    }
}
