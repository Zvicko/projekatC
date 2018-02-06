using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IDataManagment
    {
        [OperationContract]
        void Write(string id, string name);

        [OperationContract]
        Dictionary<byte[],byte[]> Read();
    }
}
