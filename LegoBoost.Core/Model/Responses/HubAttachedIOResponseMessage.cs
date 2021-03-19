using System;
using LegoBoost.Core.Model.CommunicationProtocol;
using Version = LegoBoost.Core.Model.CommunicationProtocol.Version;

namespace LegoBoost.Core.Model.Responses
{
    public class HubAttachedIOResponseMessage : ResponseMessage
    {
        public byte PortId { get; }

        public Hub.AttachedIO.Event Event { get; }
        
        // should we use short or ushort?
        public Hub.AttachedIO.Type IOTypeId { get; private set; }

        public Version HardwareRevision { get; private set; }

        public Version SoftwareRevision { get; private set; }

        public byte PortA { get; private set; }

        public byte PortB { get; private set; }

        public HubAttachedIOResponseMessage(byte[] data) : base(data)
        {
            if (MessageLength < 5) throw new Exception("Wrong Response Message type");

            PortId = MessagePayload[0];
            Event = (Hub.AttachedIO.Event)MessagePayload[1];

            switch (Event)
            {
                case Hub.AttachedIO.Event.AttachedIO:
                    if (MessageLength < 15) throw new Exception("Wrong Response Message type");
                    ReadAttachedIOEvent();
                    break;
                case Hub.AttachedIO.Event.AttachedVirtualIO:
                    if (MessageLength < 9) throw new Exception("Wrong Response Message type");
                    ReadAttachedVirtualIOEvent();
                    break;
            }
        }

        private void ReadAttachedIOEvent()
        {
            this.IOTypeId = (Hub.AttachedIO.Type) BitConverter.ToUInt16(MessagePayload.ToArray(), 2);

            // it's mentioned in the docs, that version is little endian. So the version expects the msb at last param
            this.HardwareRevision = new Version(MessagePayload[4], MessagePayload[5], MessagePayload[6], MessagePayload[7]);
            this.SoftwareRevision = new Version(MessagePayload[8], MessagePayload[9], MessagePayload[10], MessagePayload[11]);
        }

        private void ReadAttachedVirtualIOEvent()
        {
            this.IOTypeId = (Hub.AttachedIO.Type) BitConverter.ToUInt16(MessagePayload.ToArray(), 2);
            this.PortA = MessagePayload[4];
            this.PortB = MessagePayload[5];
        }
    }
}