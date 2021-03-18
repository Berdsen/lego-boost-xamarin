using System;
using System.Collections.Generic;
using LegoBoost.Core.Model.CommunicationProtocol;
using LegoBoost.Core.Model.Responses;

namespace LegoBoost.Core.Utilities
{
    public class DataCreator
    {
        public static byte[] CreateCommandBytes(byte commandByte, byte[] payload)
        {
            int length = 3 + payload.Length;
            var listOfBytes = new List<byte>() { (byte)length, 0x00, commandByte };
            listOfBytes.AddRange(payload);

            return listOfBytes.ToArray();
        }

        public static Exception CreateExceptionFromMessage(GenericErrorResponseMessage errorMessage)
        {
            switch (errorMessage.ErrorCode)
            {
                case Hub.Error.Code.Ack:
                case Hub.Error.Code.Mack:
                    return new Exception("ACK / MACK error");
                case Hub.Error.Code.BufferOverflow:
                    return new Exception("BufferOverflow error");
                case Hub.Error.Code.Timeout:
                    return new Exception("Timeout error");
                case Hub.Error.Code.CommandNotRecognized:
                    return new Exception("CommandNotRecognized error");
                case Hub.Error.Code.InvalidUse:
                    return new Exception("InvalidUse error");
                case Hub.Error.Code.Overcurrent:
                    return new Exception("Overcurrent error");
                case Hub.Error.Code.InternalError:
                    return new Exception("InternalError error");
                default:
                    return new Exception("unknown error");
            }
        }

    }
}
