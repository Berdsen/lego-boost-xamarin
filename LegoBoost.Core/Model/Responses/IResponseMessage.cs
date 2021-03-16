using System.Collections.Generic;

namespace LegoBoost.Core.Model.Responses
{
    public interface IResponseMessage
    {
        int MessageLength { get; }

        byte HubId { get; } // always 0x00

        byte MessageType { get; }

        List<byte> MessagePayload { get; }
    }
}