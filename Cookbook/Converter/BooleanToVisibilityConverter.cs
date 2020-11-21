using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Cookbook.Converter
{
    /// <summary>
    /// Converter class to make the audio button visible or collapsed.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility OnTrue { get; set; }
        public Visibility OnFalse { get; set; }

        public BooleanToVisibilityConverter()
        {
            OnFalse = Visibility.Collapsed;
            OnTrue = Visibility.Visible;
        }
        /// <summary>
        /// Converter. If value is set to English or Hungarian, the object is visible.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((string)value == "en" || (string)value == "en_US" || (string)value == "hu" || (string)value == "hu_HU") return OnTrue;
            return OnFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility == false)
                return DependencyProperty.UnsetValue;

            if ((Visibility)value == OnTrue)
                return true;
            else
                return false;
        }
    }
}
