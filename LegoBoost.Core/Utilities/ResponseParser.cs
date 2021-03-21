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
                case Hub.Property.Command:
                    return new HubPropertyResponseMessage(characteristicValue);
                case Hub.Action.Command:
                    return new HubActionResponseMessage(characteristicValue);
                case Hub.AttachedIO.Command:
                    return new HubAttachedIOResponseMessage(characteristicValue);
                case Hub.Error.Command:
                    return new GenericErrorResponseMessage(characteristicValue);
                case Hub.PortOutput.ResponseCommand:
                    // TODO: hier weitermachen
                    return new GenericErrorResponseMessage(characteristicValue);
            }

            return null;
        }
    }
}
