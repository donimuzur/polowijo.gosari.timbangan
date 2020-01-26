using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace polowijo.gosari.timbangan.dto
{
    public class MstKonsumenDto
    {
        public int ID { get; set; }
        public string NAMA_KONSUMEN { get; set; }
        public string ALAMAT_KONSUMEN { get; set; }
        public string CONTACT_PERSON { get; set; }
        public string NO_TELP { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string PERSONNO { get; set; }
    }
    public class MstKonsumenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                MstKonsumenDto myValue = (MstKonsumenDto)value;
                string description = myValue.NAMA_KONSUMEN;
                return description;
            }
            catch (Exception)
            {
                return "Please Select";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
