using System.Collections.Generic;
using System.Linq;
using LegoBoost.Core.Model.CommunicationProtocol;

namespace LegoBoost.Core.Model.Responses
{
    public class HubPropertyResponseMessage : ResponseMessage
    {
        public Hub.Property.Name Property { get; }

        public Hub.Property.Operation Method { get; }

        public new List<byte> MessagePayload { get; }

        public HubPropertyResponseMessage(byte[] data) : base(data)
        {
            Property = (Hub.Property.Name) base.MessagePayload[0];
            Method = (Hub.Property.Operation) base.MessagePayload[1];

            //               create a copy
            MessagePayload = base.MessagePayload.ToArray().ToList();
            MessagePayload.RemoveRange(0, 2);
        }
    }
}