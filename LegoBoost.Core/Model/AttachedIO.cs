namespace LegoBoost.Core.Model
{
    public abstract class AttachedIO
    {
        public CommunicationProtocol.Hub.AttachedIO.Type DeviceType { get; protected set; }
        
        public byte PortId { get; protected set; }
    }
}
