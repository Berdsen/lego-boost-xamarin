using System;
using LegoBoost.Core.Model.CommunicationProtocol;

namespace LegoBoost.Core.Model.Responses
{
    public class PortOutputFeedbackResponseMessage : ResponseMessage
    {
        public byte? PortId { get; }
        public byte? Port2Id { get; } = null;
        public byte? Port3Id { get; } = null;
        
        public Hub.PortOutputFeedback.Message PortFeedback { get; }
        public Hub.PortOutputFeedback.Message Port2Feedback { get; } = Hub.PortOutputFeedback.Message.None;
        public Hub.PortOutputFeedback.Message Port3Feedback { get; } = Hub.PortOutputFeedback.Message.None;

        public PortOutputFeedbackResponseMessage(byte[] data) : base(data)
        {
            switch (MessageLength)
            {
                case 9:
                {
                    Port3Feedback = (Hub.PortOutputFeedback.Message)MessagePayload[5];
                    Port3Id = MessagePayload[4];
                    goto case 7;
                }
                case 7:
                {
                    Port2Feedback = (Hub.PortOutputFeedback.Message)MessagePayload[3];
                    Port2Id = MessagePayload[2];
                    goto case 5;
                }
                case 5:
                {
                    PortFeedback = (Hub.PortOutputFeedback.Message)MessagePayload[1];
                    PortId = MessagePayload[0];
                    break;
                }
                default:
                    throw new Exception("Seems to be wrong message type");
            }

        }

    }
}