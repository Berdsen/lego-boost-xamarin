
using System;
using System.Globalization;
using LegoBoost.Core.Model.CommunicationProtocol;
using Xamarin.Forms;

namespace LegoBoostDemo
{
    public class HubColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.TryParse(parameter?.ToString() ?? "", true, out Hub.Color color))
            {
                return color;
            }

            return Hub.Color.Off;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.TryParse(parameter?.ToString() ?? "", true, out Hub.Color color))
            {
                return color;
            }

            return Hub.Color.Off;
        }
    }
}