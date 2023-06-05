using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DiplomHus
{
    internal class IdMasterConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {//да запускай
            return dp_hus_dipEntities2.GetContext().User
                .ToList()
                .FirstOrDefault(x => x.ID_User == (value as int?))?.ToString();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
