using System.Threading.Tasks;
using LegoBoost.Core.Model.Constants;

namespace LegoBoost.Core.Services
{
    public interface ILegoService
    {
        bool IsConnected { get; }

        Task<bool> TryConnectAsync();

        Task DisconnectAsync();
        
        Task ShutDownAsync();

        Task BlinkAsync();

        Task SetColorAsync(BoostColors color);

        Task<string> RequestDeviceNameAsync();
    }
}