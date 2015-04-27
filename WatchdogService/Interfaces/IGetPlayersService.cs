using System.ServiceModel;

namespace WatchdogService.Interfaces
{
    [ServiceContract]
    public interface IGetPlayersService
    {
        [OperationContract]
        string GetPlayers();
    }
}
