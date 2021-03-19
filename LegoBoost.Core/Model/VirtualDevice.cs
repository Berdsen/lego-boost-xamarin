namespace LegoBoost.Core.Model
{
    public class VirtualDevice : AttachedIO
    {
        public byte PortA { get; }
        
        public byte PortB { get; }

        public VirtualDevice(Core.Model.CommunicationProtocol.Hub.AttachedIO.Type deviceType, byte portId, byte portA, byte portB)
        {
            DeviceType = deviceType;
            PortId = portId;
            PortA = portA;
            PortB = portB;
        }
    }
}