using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO;

namespace Server
{
    public class DataManagment : IDataManagment
    {

        public static List<string> list
        { get; set; }

        

        public List<string> Read()
        {
            StreamReader sr = new StreamReader(@"User1.txt");
            string line;
            list = new List<string>(); // ovo sam uradio da se nebi gomilali duplikati
            while ((line = sr.ReadLine()) != null)
            {


                list.Add(line);


            }
            return list;
            //throw new NotImplementedException();
        }

        public void Write()
        {
            //throw new NotImplementedException();
        }
    }
}
