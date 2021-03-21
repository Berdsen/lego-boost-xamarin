namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public static partial class Hub
    {
        public static class PortOutput
        {
            public const byte Command = 0x81;
            public const byte ResponseCommand = 0x82;

            public enum SubCommands : byte
            {
                WriteDirectModeData = 0x51 
            }
        }
    }

}
