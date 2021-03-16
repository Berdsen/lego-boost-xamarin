using System.Collections.Generic;
using System.Linq;

namespace LegoBoost.Core.Model.Responses
{
    public class HubPropertyResponseMessage : ResponseMessage
    {
        public byte Property { get; }

        public byte Method { get; }

        public new List<byte> MessagePayload { get; }

        public HubPropertyResponseMessage(byte[] data) : base(data)
        {
            Property = base.MessagePayload[0];
            Method = base.MessagePayload[1];

            //               create a copy
            MessagePayload = base.MessagePayload.ToArray().ToList();
            MessagePayload.RemoveRange(0, 2);
        }
    }
}