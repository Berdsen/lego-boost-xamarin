using System;

namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public static partial class Hub
    {
        public static class PortOutputFeedback
        {
            [Flags]
            public enum Message : byte
            {
                None = 0x00, // Self introduced for better working with optional feedback
                BufferEmptyCommandInProgress = 0x01,
                BufferEmptyCommandInCompleted = 0x02,
                CurrentCommandDiscarded = 0x04,
                Idle = 0x08,
                BusyFull = 0x10
            }
        }
    }
}