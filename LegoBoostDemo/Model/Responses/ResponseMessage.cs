using System.Collections.Generic;
using System.Linq;

namespace LegoBoostDemo.Model.Responses
{
    public abstract class ResponseMessage
    {
        public int Length { get; }

        public byte HubId { get; } // always 0x00

        public byte MessageType { get; }

        public List<byte> MessagePayload { get; protected set; }

        public ResponseMessage(byte[] data)
        {
            Length = (int)data[0];
            HubId = data[1];
            MessageType = data[2];

            MessagePayload = data.ToList();
            MessagePayload.RemoveRange(0, 3);
        }
    }
}