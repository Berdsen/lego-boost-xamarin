using LegoBoost.Core.Model;
using LegoBoost.Core.Model.CommunicationProtocol;
using CPHub = LegoBoost.Core.Model.CommunicationProtocol.Hub;

namespace LegoBoost.Xamarin.Model
{
    public class RealDevice : IRealDevice
    {
        public Core.Model.CommunicationProtocol.Hub.AttachedIO.Type DeviceType { get; }

        public byte PortId { get; }

        public Version HardwareVersion { get; }

        public Version SoftwareVersion { get; }

        public RealDevice(CPHub.AttachedIO.Type type, byte portId, Version hwVersion, Version swVersion)
        {
            DeviceType = type;
            PortId = portId;
            HardwareVersion = hwVersion;
            SoftwareVersion = swVersion;
        }

    }
}