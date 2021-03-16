using LegoBoost.Core.Model.Responses;

namespace LegoBoost.Core.Utilities
{
    public class ResponseParser
    {
        public static ResponseMessage ParseMessage(byte[] characteristicValue)
        {
            if (characteristicValue == null || characteristicValue.Length < 3) return null;

            switch (characteristicValue[2])
            {
                case 0x01:
                    return new HubPropertyResponseMessage(characteristicValue);
                case 0x02:
                    return new HubActionResponseMessage(characteristicValue);
            }

            return null;
        }
    }
}
