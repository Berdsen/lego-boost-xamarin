using System.Threading.Tasks;

namespace LegoBoostDemo.Services
{
    public interface ILegoService
    {
        bool IsConnected { get; }

        Task<bool> TryConnectAsync();

        Task TryDisconnectAsync();

        Task BlinkAsync();

        Task SetColorAsync(BoostColors color);
    }
}