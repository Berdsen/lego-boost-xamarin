
using LegoBoost.Core.Model.CommunicationProtocol;

namespace LegoBoost.Core.Model
{
    public class RealDevice : AttachedIO
    {
        public Version HardwareVersion { get; }

        public Version SoftwareVersion { get; }
        
        public RealDevice(Core.Model.CommunicationProtocol.Hub.AttachedIO.Type deviceType, byte portId, Version hwVersion, Version swVersion )
        {
            DeviceType = deviceType;
            PortId = portId;
            HardwareVersion = hwVersion;
            SoftwareVersion = swVersion;
        }
    }
}