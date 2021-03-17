namespace LegoBoost.Core.Model.Constants
{
    public static class ErrorCodes
    {
        public const byte Command = 0x05;

        public static class ErrorCodeBytes
        {
            public const byte Ack = 0x01;
            public const byte Mack = 0x02;
            public const byte BufferOverflow = 0x03;
            public const byte Timeout = 0x04;
            public const byte CommandNotRecognized = 0x05;
            public const byte InvalidUse = 0x06;
            public const byte Overcurrent = 0x07;
            public const byte InternalError = 0x08;
        }
    }
}