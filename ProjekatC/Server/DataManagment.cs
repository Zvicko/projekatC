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
        public void Write(string id, string name)
        {
            if (File.Exists("Informations.xml") == true)     // Provera postojanja datoteke u kojoj se dodaje specificiran element.
            {
                XDocument xDocument = XDocument.Load("Informations.xml");           // Ukoliko datoteka postoji, ucitaju se podaci iz nje.
                XElement root = xDocument.Element("Elements");                      // Pribavlja se cvor koji predstavlja koren datoteke.
                IEnumerable<XElement> rows = root.Descendants("Element");           // Lista cvorova potomaka korenog elementa.
                XElement lastRow = rows.Last();                                     // Odredjivanje poslednjeg elementa iz liste.

                lastRow.AddAfterSelf(new XElement("Element", new XElement("ID", id), new XElement("Name", name)));  // Po odredjinanju poslednjeg elementa, nakon tog elementa doda se element na osnovu specificiranih vrednosti.

                xDocument.Save("Informations.xml");                                 // Nakon azuriranja .xml datoteke, promene se sacuvaju.
            }
            else
            {
                using (XmlWriter xmlWriter = XmlWriter.Create("Informations.xml"))  // Ukoliko specificirana datoteka ne postoji, pravi se nova .xml datoteka.     
                {
                    xmlWriter.WriteStartDocument();                 // Pocetni element svake .xml datoteke.

                    xmlWriter.WriteStartElement("Elements");        // Koreni element .xml datoteke.

                    xmlWriter.WriteStartElement("Element");         // Dodavanje specificiranog elementa.
                    xmlWriter.WriteElementString("ID", id);
                    xmlWriter.WriteElementString("Name", name);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndDocument();

                    xmlWriter.Flush();                              // Praznjenje bafera.
                    xmlWriter.Close();                              // Zatvaranje XmlWriter-a.
                }
            }
        }

        // Metoda koja cita .xml datoteku i vraca listu pronadjenih elemenata.
        public List<Element> Read()
        {
            string id = "";                         // Promenljiva id cuva id pronadjenog elementa.
            string name = "";                       // Promenljiva name cuva ime pronadjenog elementa.
            bool element_added = true;              // Promenljiva element_added javlja kada je element dodat.

            List<Element> elements = new List<Element>();

            if (File.Exists("Informations.xml") == true)
            {
                XmlReader xmlReader = XmlReader.Create("Informations.xml");

                while (xmlReader.Read())
                {
                    if (id != "" && name != "")                     // Kada su promenljive id i name neprazni stringovi novi element je pronadjen.
                    {
                        Element element = new Element(id, name);    // Pravljenje novog elementa na osnovu procitanih vrednosti ID-a i imena.

                        elements.Add(element);                      // Dodavanje elementa u listu elemenata.

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
