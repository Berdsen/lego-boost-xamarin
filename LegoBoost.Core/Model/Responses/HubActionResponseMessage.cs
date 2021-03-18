using System.Collections.Generic;
using System.Linq;
using LegoBoost.Core.Model.CommunicationProtocol;

namespace LegoBoost.Core.Model.Responses
{
    public class HubActionResponseMessage : ResponseMessage
    {
        public Hub.Action.Name Action { get; }

        public new List<byte> MessagePayload { get; }

        public HubActionResponseMessage(byte[] data) : base(data)
        {
            Action = (Hub.Action.Name) base.MessagePayload[0];

            //               create a copy
            MessagePayload = base.MessagePayload.ToArray().ToList();
            MessagePayload.RemoveRange(0, 1);
        }
    }
}