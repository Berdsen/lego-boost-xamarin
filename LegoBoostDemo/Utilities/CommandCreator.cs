using System.Collections.Generic;

namespace LegoBoostDemo.Utilities
{
    public class CommandCreator
    {
        public static byte[] CreateCommandBytes(byte commandByte, byte[] payload)
        {
            int length = 3 + payload.Length;
            var listOfBytes = new List<byte>() { (byte)length, 0x00, commandByte };
            listOfBytes.AddRange(payload);

            return listOfBytes.ToArray();
        }
    }
}
