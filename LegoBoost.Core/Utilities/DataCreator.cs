using System;
using System.Collections.Generic;
using LegoBoost.Core.Model.Constants;
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
                case ErrorCodes.ErrorCodeBytes.Ack:
                case ErrorCodes.ErrorCodeBytes.Mack:
                    return new Exception("ACK / MACK error");
                case ErrorCodes.ErrorCodeBytes.BufferOverflow:
                    return new Exception("BufferOverflow error");
                case ErrorCodes.ErrorCodeBytes.Timeout:
                    return new Exception("Timeout error");
                case ErrorCodes.ErrorCodeBytes.CommandNotRecognized:
                    return new Exception("CommandNotRecognized error");
                case ErrorCodes.ErrorCodeBytes.InvalidUse:
                    return new Exception("InvalidUse error");
                case ErrorCodes.ErrorCodeBytes.Overcurrent:
                    return new Exception("Overcurrent error");
                case ErrorCodes.ErrorCodeBytes.InternalError:
                    return new Exception("InternalError error");
                default:
                    return new Exception("unknown error");
            }
        }

    }
}
