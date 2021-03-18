using System;
using LegoBoost.Core.Model.CommunicationProtocol;

namespace LegoBoost.Core.Model.Responses
{
    public class HubAttachedIOResponseMessage : ResponseMessage
    {
        public byte PortId { get; }

        public Hub.AttachedIO.Event Event { get; }
        
        // should we use short or ushort?
        public Hub.AttachedIO.Type IOTypeId { get; private set; }

        public uint HardwareRevision { get; private set; }

        public uint SoftwareRevision { get; private set; }

        public byte PortIdA { get; private set; }

        public byte PortIdB { get; private set; }

        public HubAttachedIOResponseMessage(byte[] data) : base(data)
        {
            if (MessageLength < 5) throw new Exception("Wrong Response Message type");

            PortId = MessagePayload[0];
            Event = (Hub.AttachedIO.Event) Enum.Parse(typeof(Hub.AttachedIO.Event), MessagePayload[1].ToString());

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
            this.HardwareRevision = BitConverter.ToUInt32(MessagePayload.ToArray(), 4);
            this.SoftwareRevision = BitConverter.ToUInt32(MessagePayload.ToArray(), 8);
        }

        private void ReadAttachedVirtualIOEvent()
        {
            this.IOTypeId = (Hub.AttachedIO.Type) BitConverter.ToUInt16(MessagePayload.ToArray(), 2);
            this.PortIdA = MessagePayload[4];
            this.PortIdB = MessagePayload[5];
        }
    }
}