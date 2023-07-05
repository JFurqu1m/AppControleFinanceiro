using AppFinanceiro.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFinanceiro.Properties.Libraries.Converters
{
    public class TransactionValueColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Transaction transaction = (Transaction)value;

            if (transaction == null)
            {
                return Colors.Black;
            }
            if (transaction.Type == TransactionType.Income)
            {
                return Color.FromArgb("#FF939E5A");
            }
            else
            {
                return Colors.Red;
            }


            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
