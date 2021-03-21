namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public static partial class Hub
    {
        public static class MessageCommand
        {
            public const byte Property = 0x01;
            public const byte Action = 0x02;
            public const byte AttachedIO = 0x04;
            public const byte Error = 0x05;
            public const byte PortOutput = 0x81;
            public const byte PortOutputFeedback = 0x82;
        }

    }
}