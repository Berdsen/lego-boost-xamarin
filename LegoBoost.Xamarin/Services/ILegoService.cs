using System.Collections.Generic;
using System.Threading.Tasks;
using LegoBoost.Core.Model;
using LegoBoost.Core.Model.CommunicationProtocol;

namespace LegoBoost.Xamarin.Services
{
    public interface ILegoService
    {
        bool IsConnected { get; }

        Task<bool> TryConnectAsync();

        Task DisconnectAsync();
        
        Task ShutDownAsync();

        Task BlinkAsync();

        Task<bool> SetColorAsync(Hub.Color color);

        Task<string> RequestDeviceNameAsync();

        Task<string> SetDeviceNameAsync(string newDeviceName);

        List<IAttachedIO> GetIODevices();
    }
}