using System.Threading.Tasks;
using LegoBoostDemo.Model;
using LegoBoostDemo.Model.Constants;

namespace LegoBoostDemo.Services
{
    public interface ILegoService
    {
        bool IsConnected { get; }

        Task<bool> TryConnectAsync();

        Task TryDisconnectAsync();

        Task BlinkAsync();

        Task SetColorAsync(BoostColors color);

        Task<string> RequestDeviceNameAsync();
    }
}