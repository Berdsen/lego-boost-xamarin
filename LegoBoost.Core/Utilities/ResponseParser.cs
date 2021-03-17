using LegoBoost.Core.Model.Constants;
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
                case HubProperties.Command:
                    return new HubPropertyResponseMessage(characteristicValue);
                case HubActions.Command:
                    return new HubActionResponseMessage(characteristicValue);
                case HubAttachedIO.Command:
                    return new HubAttachedIOResponseMessage(characteristicValue);
                case ErrorCodes.Command:
                    return new GenericErrorResponseMessage(characteristicValue);
            }

            return null;
        }
    }
}
