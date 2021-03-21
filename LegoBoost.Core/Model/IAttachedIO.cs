using System.Threading.Tasks;
using LegoBoost.Core.Model.CommunicationProtocol;
using LegoBoost.Core.Model.Responses;

namespace LegoBoost.Core.Model
{
    public interface IAttachedIO
    {
        CommunicationProtocol.Hub.AttachedIO.Type DeviceType { get; }
        
        byte PortId { get; }

        Task<HubPropertyResponseMessage> PortOutputCommandAsync(StartupAndCompletionInfo startupAndCompletionInfo, Hub.PortOutput.SubCommands subCommand, byte[] payload);
    }
}
