using System.Collections.Generic;
using System.Threading.Tasks;
using LegoBoost.Core.Model;
using LegoBoost.Core.Model.CommunicationProtocol;
using LegoBoost.Core.Model.Responses;
using LegoBoost.Core.Utilities;
using LegoBoost.Xamarin.Utilities;
using CPHub = LegoBoost.Core.Model.CommunicationProtocol.Hub;

namespace LegoBoost.Xamarin.Model
{
    public class VirtualDevice : IVirtualDevice
    {
        public Core.Model.CommunicationProtocol.Hub.AttachedIO.Type DeviceType { get; }
        
        public byte PortId { get; }
        
        public byte PortA { get; }
        
        public byte PortB { get; }

        public VirtualDevice(Core.Model.CommunicationProtocol.Hub.AttachedIO.Type type, byte portId, byte portA, byte portB)
        {
            DeviceType = type;
            PortId = portId;
            PortA = portA;
            PortB = portB;
        }

    }
}