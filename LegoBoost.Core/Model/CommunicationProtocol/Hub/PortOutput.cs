namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public static partial class Hub
    {
        public static class PortOutput
        {
            public enum SubCommands : byte
            {
                WriteDirectModeData = 0x51 
            }
        }
    }

}
