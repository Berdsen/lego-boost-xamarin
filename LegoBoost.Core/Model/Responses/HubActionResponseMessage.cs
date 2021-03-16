using System.Collections.Generic;
using System.Linq;

namespace LegoBoost.Core.Model.Responses
{
    public class HubActionResponseMessage : ResponseMessage
    {
        public byte Action { get; }

        public new List<byte> MessagePayload { get; }

        public HubActionResponseMessage(byte[] data) : base(data)
        {
            Action = base.MessagePayload[0];

            //               create a copy
            MessagePayload = base.MessagePayload.ToArray().ToList();
            MessagePayload.RemoveRange(0, 1);
        }
    }
}