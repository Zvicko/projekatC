using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using CertificateController;
using Common;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:5500/DataManagment";

            int option = 0;                 // Promenljiva option u kojoj se skladisti izabrana opcija.
            string opt = "";                // Promenljiva opt u kojoj se skladisti string reprezentacija odabrane opcije.
            bool isOptValid = false;        // Promenljiva isOptValid u kojoj se belezi da je izabrana opcija validna.

            string privateKey = "burek";    // Kljuc za enkripciju i dekripciju podataka.
            byte[] keyb = Encoding.ASCII.GetBytes(privateKey);

            string clientCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            X509Certificate2 clientCert = CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientCertCN);

            if (clientCert != null)
            {
                AuthenticationService authService = new AuthenticationService();

                Console.WriteLine("");

                using (ClientProxy clp = new ClientProxy(binding, address))
                {
                    while (option != 3)
                    {
                        Console.WriteLine("\t\t\t\t\t|=====================================|");        // Ispis glavnog menija.
                        Console.WriteLine("\t\t\t\t\t|              MAIN MENU              |");
                        Console.WriteLine("\t\t\t\t\t|-------------------------------------|");
                        Console.WriteLine("\t\t\t\t\t|                                     |");
                        Console.WriteLine("\t\t\t\t\t|              1. Write               |");
                        Console.WriteLine("\t\t\t\t\t|              2. Read                |");
                        Console.WriteLine("\t\t\t\t\t|              3. Exit                |");
                        Console.WriteLine("\t\t\t\t\t|                                     |");
                        Console.WriteLine("\t\t\t\t\t|=====================================|\n\n");

                        do  // Otkrivanje nevalidne vrednosti opcije.
                        {
                            Console.Write("Enter option number: ");
                            opt = Console.ReadLine();

                            isOptValid = int.TryParse(opt, out option);

                            if (isOptValid == false)                // Slucaj kada odabrana opcija nije numericka vrednost.
                            {
                                Console.WriteLine("\n---------------------");
                                Console.WriteLine("Invalid option value.");
                                Console.WriteLine("---------------------\n");
                            }
                            else if (option < 1 || option > 3)      // Odabrana opcija je van opsega. Odabir nepostojece opcije.
                            {
                                Console.WriteLine("\n----------------------------------");
                                Console.WriteLine("The entered option does not exist.");
                                Console.WriteLine("----------------------------------\n");
                            }

                        } while (isOptValid == false || option < 1 || option > 3);

                        switch (option)
                        {
                            case 1:
                                {
                                    string id = "";         // Promenljiva id u kojoj se skladisti vrednost za ID elementa.
                                    string name = "";       // Promenljiva name u kojoj se skladisti vrednost za ime elementa. 

                                    Console.WriteLine("\n=======================================================================================================================");
                                    Console.WriteLine("\t\t\t\t\t\t\t WRITE");       // Naznaka korisniku da je program usao u odabranu opciju.
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------\n");

                                    do                                      // ID mora da ima najmanje 4 karaktera.
                                    {
                                        Console.Write("Enter ID: ");        // Unosenje i skladistenje ID-a elementa.
                                        id = Console.ReadLine();

                                        if (id.Length < 4)
                                        {
                                            Console.WriteLine("\n---------------------------------------");
                                            Console.WriteLine("The ID must have at least 4 characters.");
                                            Console.WriteLine("---------------------------------------\n");
                                        }

                                    } while (id.Length < 4);

                                    do                                      // Ime mora da ima najmanje 3 karaktera.
                                    {
                                        Console.Write("\nEnter name: ");    // Unosenje i skladistenje imena elementa.
                                        name = Console.ReadLine();

                                        if (name.Length < 3)
                                        {
                                            Console.WriteLine("\n-----------------------------------------");
                                            Console.WriteLine("The name must have at least 3 characters.");
                                            Console.WriteLine("-----------------------------------------");
                                        }

                                    } while (name.Length < 3);

                                    byte[] idB = Encoding.ASCII.GetBytes(id);       // Enkodiranje ID-a i naziva elementa u niz byte-ova.
                                    byte[] nameB = Encoding.ASCII.GetBytes(name);

                                    byte[] idEnc = RC4.Encrypt(keyb, idB);          // Enkriptovanje ID-a i naziva elementa.
                                    byte[] nameEnc = RC4.Encrypt(keyb, nameB);

                                    clp.Write(idEnc, nameEnc);                      // Prosledjivanje enkriptovanih podataka.

                                    Console.WriteLine("\n-----------------------------------------------------------------------------------------------------------------------");
                                    Console.WriteLine("\t\t\t\t   The specified element has been sucessfully added.");
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");

                                    Console.WriteLine("\n=======================================================================================================================\n\n");     // Naznaka korisniku da je zavrsen rad sa prvom opcijom.

                                    break;              // Zavrsetak prve opcije.
                                }

                            case 2:                     // Prikazivanje svih elemenata koji se trenutno nalaze u bazi podataka.
                                {
                                    Console.WriteLine("\n=======================================================================================================================");
                                    Console.WriteLine("\t\t\t\t\t\t\t READ");
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------\n");

                                    Dictionary<byte[], byte[]> dic = clp.Read();              // Poziva se metoda za citanje elemenata.

                                    if (dic.Count == 0)            // Ukoliko baza podataka ne postoji korisnik se obavestava o tome.
                                    {
                                        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
                                        Console.WriteLine("\t\t       The data base does not exist. It will be created when an element is added.");
                                        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------\n");
                                    }
                                    else
                                    {

                                        foreach (KeyValuePair<byte[], byte[]> kvp in dic)     // Ispis pronadjenih elemenata.
                                        {
                                            byte[] keyDecrBytes = RC4.Decrypt(keyb, kvp.Key);
                                            byte[] valueDecrBytes = RC4.Decrypt(keyb, kvp.Value);

                                            string idDecr = Encoding.ASCII.GetString(keyDecrBytes);
                                            string valueDecr = Encoding.ASCII.GetString(valueDecrBytes);

                                            Console.WriteLine("------------------------------");
                                            Console.WriteLine("ID: {0}", idDecr);
                                            Console.WriteLine("Name: {0}", valueDecr);
                                            Console.WriteLine("------------------------------\n");
                                        }
                                    }

                                    Console.WriteLine("=======================================================================================================================\n\n");

                                    break;
                                }

                            case 3:                     // Izlazak iz programa.
                                {
                                    clp.Close();

                                    Console.WriteLine("\nPress any key to exit...");
                                    Console.ReadLine();

                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Client authentication falied. No certificate provided. Client can not be started.\n");
                Console.WriteLine("Press ENTER key to exit...");
                Console.ReadLine();
            }
        }
    }
}
