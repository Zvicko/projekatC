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

        public void Write(byte[] id, byte[] name)
        {
            factory.Write(id, name);
        }

        public Dictionary<byte[], byte[]> Read()
        {
            return factory.Read();   
        }
    }
}
