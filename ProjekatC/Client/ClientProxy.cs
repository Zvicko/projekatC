using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.ServiceModel;
namespace Client
{
    public class ClientProxy : ChannelFactory<IDataManagment>, IDataManagment, IDisposable
    {
        IDataManagment factory;
       
        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public List<Element> Read()
        {
            List<Element> elements = new List<Element>(0);

            return elements;
        }

        public void Write(string id, string name)
        {
            factory.Write(id, name);
        }
    }
}
