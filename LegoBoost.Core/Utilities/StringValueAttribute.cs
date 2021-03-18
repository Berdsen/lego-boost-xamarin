using System;

namespace LegoBoost.Core.Utilities
{
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; }

        public StringValueAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(value);

            StringValue = value;
        }

    }
}