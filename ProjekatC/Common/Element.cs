using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Element
    {
        private string id;
        private string name;

        public Element()
        {

        }

        public Element(string Id, string n)
        {
            id = Id;
            name = n;
        }

        [DataMember]
        public string ID
        {
            get { return id; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
        }
    }
}
