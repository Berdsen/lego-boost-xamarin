namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public static partial class Hub
    {
        public static class Error
        {
            public enum Code : byte
            {
                Ack = 0x01,
                Mack = 0x02,
                BufferOverflow = 0x03,
                Timeout = 0x04,
                CommandNotRecognized = 0x05,
                InvalidUse = 0x06,
                Overcurrent = 0x07,
                InternalError = 0x08
            }
        }
    }

}