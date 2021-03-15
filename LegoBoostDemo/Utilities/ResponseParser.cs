using LegoBoostDemo.Model.Responses;

namespace LegoBoostDemo.Utilities
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
            }

            return null;
        }
    }
}
