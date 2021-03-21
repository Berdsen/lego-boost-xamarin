using LegoBoost.Core.Model.CommunicationProtocol;
using LegoBoost.Core.Model.Responses;

namespace LegoBoost.Core.Utilities
{
    public class ResponseParser
    {
        public static IResponseMessage ParseMessage(byte[] characteristicValue)
        {
            if (characteristicValue == null || characteristicValue.Length < 3) return null;

            switch (characteristicValue[2])
            {
                case Hub.MessageCommand.Property:
                    return new HubPropertyResponseMessage(characteristicValue);
                case Hub.MessageCommand.Action:
                    return new HubActionResponseMessage(characteristicValue);
                case Hub.MessageCommand.AttachedIO:
                    return new HubAttachedIOResponseMessage(characteristicValue);
                case Hub.MessageCommand.Error:
                    return new GenericErrorResponseMessage(characteristicValue);
                case Hub.MessageCommand.PortOutputFeedback:
                    return new PortOutputFeedbackResponseMessage(characteristicValue);
            }

            return null;
        }
    }
}
