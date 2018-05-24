using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ImageService.Logging.Modal;

namespace ImageServiceGUI.View.Convertors
{
    class LogStatusToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), value.ToString()))
            {
                case MessageTypeEnum.INFO: return Brushes.Green;
                case MessageTypeEnum.FAIL: return Brushes.Red;
                case MessageTypeEnum.WARNING: return Brushes.Yellow;
                default: return Brushes.LightGray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
