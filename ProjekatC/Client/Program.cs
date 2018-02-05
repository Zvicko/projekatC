using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Common;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:5500/DataManagment";
            List<string> list = new List<string>();

            int option = 0;                 // Promenljiva option je promenljiva u kojoj se skladisti izabrana opcija.
            string opt = "";                // Promenljiva opt je promenljiva u kojoj se skladisti string reprezentacija odabrane opcije.
            bool isOptValid = false;        // Promenljiva isOptValid je promenljiva u kojoj se belezi da je izabrana opcija validna.

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

                                do                                      // Ime mora da ima najmanje 6 karaktera.
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

                                clp.Write(id, name);                 // Poziv metode Write koja dodaje jedan element u .xml datoteku.

                                Console.WriteLine("\n-----------------------------------------------------------------------------------------------------------------------");
                                Console.WriteLine("\t\t\t\t   The specified element has been sucessfully added.");
                                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");

                                Console.WriteLine("\n=======================================================================================================================\n\n");     // Naznaka korisniku da je zavrsen rad sa prvom opcijom.

                                break;              // Zavrsetak prve opcije.
                            }

                        case 2:                     // Prikazivanje svih elemenata koji se trenutno nalaze u .xml datoteci.
                            {
                                Console.WriteLine("\n=======================================================================================================================");
                                Console.WriteLine("\t\t\t\t\t\t\t READ");
                                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------\n");

                                List<Element> elements = new List<Element>();

                                elements = clp.Read();              // Poziva se metoda za citanje elemenata.

                                foreach (Element e in elements)     // Ispis pronadjenih elemenata.
                                {
                                    Console.WriteLine("------------------------------");
                                    Console.WriteLine("ID: {0}", e.ID);
                                    Console.WriteLine("Name: {0}", e.Name);
                                    Console.WriteLine("------------------------------\n");
                                }

                                Console.WriteLine("=======================================================================================================================\n\n");

                                break;
                            }

                        case 3:                     // Izlazak iz programa.
                            {
                                clp.Close();

                                break;
                            }
                    }
                }   
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
