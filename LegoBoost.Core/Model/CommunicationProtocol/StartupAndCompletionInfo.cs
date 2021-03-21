namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public class StartupAndCompletionInfo
    {
        public enum Startup : byte
        {
            BufferIfNecessary = 0x00,
            ExecuteImmediately = 0x01
        }

        public enum Completion : byte
        {
            NoAction = 0x00,
            CommandFeedback = 0x01
        }

        public Startup StartupPart { get; }

        public Completion CompletionPart { get; }

        public byte InfoByte => (byte) ((byte) StartupPart << 4 | (byte)CompletionPart);

        public StartupAndCompletionInfo(Startup startup, Completion completion)
        {
            this.StartupPart = startup;
            this.CompletionPart = completion;
        }

    }
}
