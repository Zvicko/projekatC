using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Security;

namespace CertificateController
{
    public class CertificateManager
    {
        /// <summary>
        /// Pribavljanje sertifikata sa specificiranim imenom iz skladista.
        /// </summary>
        /// <param name="storeName"> Naziv skladista sertifikata. </param>
        /// <param name="storeLocation"> Lokacija skladista sertifikata. </param>
        /// <param name="subjectName"> Ime subjekta. </param>
        /// <returns> U slucaju nalazenja sertifikata vraca nadjen sertifikat, u suprotnom null. </returns>
        public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);

            foreach (X509Certificate2 cert in certCollection)
            {
                if (cert.SubjectName.Name.Equals(string.Format("CN={0}", subjectName)))
                {
                    return cert;
                }
            }

            return null;
        }
    }
}
