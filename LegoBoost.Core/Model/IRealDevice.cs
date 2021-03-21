
using LegoBoost.Core.Model.CommunicationProtocol;

namespace LegoBoost.Core.Model
{
    public interface IRealDevice : IAttachedIO
    {
        Version HardwareVersion { get; }

        Version SoftwareVersion { get; }

    }
}