using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Server
{
    public class DataManagment : IDataManagment
    {
        // Write dodaje jedan novi element u .xml datoteku na osnovu prosedjenog ID-a i imena elementa.
        public void Write(byte[] id, byte[] name)
        {
            string key = "burek";
            byte[] keyB = Encoding.ASCII.GetBytes(key);
            byte[] idDec = RC4.Decrypt(keyB, id);
            byte[] nameDec = RC4.Decrypt(keyB, name);
            string idString = Encoding.ASCII.GetString(idDec);
            string nameString = Encoding.ASCII.GetString(nameDec);


            if (File.Exists("Informations.xml"))     // Provera postojanja datoteke u kojoj se dodaje specificiran element.
            {
                XDocument xDocument = XDocument.Load("Informations.xml");           // Ukoliko datoteka postoji, ucitaju se podaci iz nje.
                XElement root = xDocument.Element("Elements");                      // Pribavlja se cvor koji predstavlja koren datoteke.
                IEnumerable<XElement> rows = root.Descendants("Element");           // Lista cvorova potomaka korenog elementa.
                XElement lastRow = rows.Last();                                     // Odredjivanje poslednjeg elementa iz liste.

                lastRow.AddAfterSelf(new XElement("Element", new XElement("ID", idString), new XElement("Name", nameString)));  // Po odredjinanju poslednjeg elementa, nakon tog elementa doda se element na osnovu specificiranih vrednosti.

                xDocument.Save("Informations.xml");                                 // Nakon azuriranja .xml datoteke, promene se sacuvaju.
            }
            else
            {
                using (XmlWriter xmlWriter = XmlWriter.Create("Informations.xml"))  // Ukoliko specificirana datoteka ne postoji, pravi se nova .xml datoteka.     
                {
                    xmlWriter.WriteStartDocument();                 // Pocetni element svake .xml datoteke.

                    xmlWriter.WriteStartElement("Elements");        // Koreni element .xml datoteke.

                    xmlWriter.WriteStartElement("Element");         // Dodavanje specificiranog elementa.
                    xmlWriter.WriteElementString("ID", idString);
                    xmlWriter.WriteElementString("Name", nameString);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndDocument();

                    xmlWriter.Flush();                              // Praznjenje bafera.
                    xmlWriter.Close();                              // Zatvaranje XmlWriter-a.
                }
            }
        }

        // Metoda koja cita .xml datoteku i vraca listu pronadjenih elemenata.
        public Dictionary<byte[],byte[]> Read()
        {
            string id = "";                         // Promenljiva id cuva id pronadjenog elementa.
            string name = "";                       // Promenljiva name cuva ime pronadjenog elementa.
            bool element_added = true;              // Promenljiva element_added javlja kada je element dodat.
            byte[] b = new byte[1];
            Dictionary<byte[],byte[]> elements = new Dictionary<byte[], byte[]>();

            if (File.Exists("Informations.xml"))
            {
                XmlReader xmlReader = XmlReader.Create("Informations.xml");

                while (xmlReader.Read())
                {
                    if (id != "" && name != "")                     // Kada su promenljive id i name neprazni stringovi novi element je pronadjen.
                    {
                        string key = "burek";
                        byte[] keyB = Encoding.ASCII.GetBytes(key);
                        byte[] nameB = Encoding.ASCII.GetBytes(name);
                        byte[] idB = Encoding.ASCII.GetBytes(id);
                        byte[] encrpytedName =  RC4.Encrypt(keyB, nameB);
                        byte[] encrpytedId = RC4.Encrypt(keyB, idB);
                        //return encrpyted;
                        //Element element = new Element(id, name);    // Pravljenje novog elementa na osnovu procitanih vrednosti ID-a i imena.

                        elements.Add(encrpytedId,encrpytedName);                      // Dodavanje elementa u listu elemenata.

                        if (element_added == true)                  // Resetovanje vrednosti promenljivih id i name nakon sto je element pronadjen.
                        {
                            id = "";
                            name = "";
                        }
                    }
                    else if (xmlReader.Name == "ID")
                    {
                        xmlReader.Read();

                        id = xmlReader.Value;

                        xmlReader.Read();
                    }
                    else if (xmlReader.Name == "Name")
                    {
                        xmlReader.Read();

                        name = xmlReader.Value;

                        xmlReader.Read();
                    }
                }

                xmlReader.Close();                  // Zatvaranje XmlReader-a.
            }

            return elements;                        // Vraca se lista pronadjenih elemenata.                        
        }
    }
}
