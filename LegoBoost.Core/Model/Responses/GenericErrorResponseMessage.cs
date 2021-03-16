using System.Linq;

namespace LegoBoost.Core.Model.Responses
{
    public class GenericErrorResponseMessage : ResponseMessage
    {
        public byte[] IssuedCommand { get; }

        public byte ErrorCode { get; }

        public GenericErrorResponseMessage(byte[] data) : base(data)
        {
            if (MessagePayload.Count == 0) return;

            ErrorCode = MessagePayload.Last();
            IssuedCommand = MessagePayload.GetRange(0, MessagePayload.Count - 1).ToArray();
        }
    }
}