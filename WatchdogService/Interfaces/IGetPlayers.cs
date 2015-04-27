using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogService.Interfaces
{
    [ServiceContract]
    public interface IGetPlayers
    {
        [OperationContract]
        [WebGet(ResponseFormat= WebMessageFormat.Json)]
        string GetPlayerList();
    }
}
