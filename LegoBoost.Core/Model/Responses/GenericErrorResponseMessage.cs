using System.Linq;
using LegoBoost.Core.Model.CommunicationProtocol;

namespace LegoBoost.Core.Model.Responses
{
    public class GenericErrorResponseMessage : ResponseMessage
    {
        public byte[] IssuedCommand { get; }

        public Hub.Error.Code ErrorCode { get; }

        public GenericErrorResponseMessage(byte[] data) : base(data)
        {
            if (MessagePayload.Count == 0) return;

            ErrorCode = (Hub.Error.Code) MessagePayload.Last();
            IssuedCommand = MessagePayload.GetRange(0, MessagePayload.Count - 1).ToArray();
        }
    }
}