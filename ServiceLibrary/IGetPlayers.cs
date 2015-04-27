using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceLibrary
{
    [ServiceContract]
    public interface IGetPlayers
    {
        [OperationContract]
        string GetPlayerList();
    }
}
