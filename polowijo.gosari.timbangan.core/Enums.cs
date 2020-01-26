using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace polowijo.gosari.timbangan.core
{
    public class Enums
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
    public enum JenisBarang
    {
        [Description("Barang Jadi")]
        BarangJadi = 1,
        [Description("Bahan Baku")]
        BahanBaku = 2
    }
    public enum Role
    {
        Administrator = 1000,
        Manager = 5000,
        Operator = 50001,
        Timbangan = 50002,
    }
    public enum MenuList
    {
        [Description("Login")]
        Login = 0,
        [Description("Home")]
        Home = 1,
        [Description("Transaksi")]
        Transaction = 2,
        [Description("Gudang")]
        Gudang = 3,
        [Description("Master")]
        Master = 4,
        [Description("Report")]
        Report = 5,
        [Description("Error")]
        Error = 6,
        [Description("Setting")]
        Setting = 7,
        [Description("Ekspedisi")]
        Ekspedisi = 8,
        [Description("Produksi")]
        Produksi = 9,

        TrnSpb = 20,
        TrnDo = 21,

        GdgBarangJadi = 30,
        GdgBarangBB = 31,
        Mutasi = 32,

        MasterWilayah = 40,
        MasterSupplier = 41,
        MasterKonsumen = 42,
        MasterTransportir = 43,
        MasterUom = 44,
        MasterKemasan = 45,
        MstBarangJadi = 46,
        MstBahanBaku = 47,

        LaporanOutstanding = 50,
        LaporanRekapBulanan = 51,
        LaporanRealisasiHarian = 52,

        Timbangan = 60,

        SettingUser = 70,

        TrnPengiriman = 80,
        TrnSuratPengantarBongkarMuat = 81,
        TrnSuratMutasiBarang = 82,
        TrnPenerimaanBarang = 83,
        TrnSlipTimbangan = 84,

        TrnHasilProduksi = 90,
        TrnPermintaanBahanBaku = 91,
        TrnSuratPerintahProduksi = 92,
        LaporanProduksi = 95,

    }
    public enum StatusDocument
    {
        [Description("Open")]
        Open = 0,
        [Description("Closed")]
        Closed = 1,
        [Description("Cancel")]
        Cancel = 2,
    }
    public enum Kemasan
    {
        [Description("25 Kg/zak")]
        kg25,
        [Description("50 Kg/zak")]
        kg50,
        [Description("Jumbo Bag / Hi Blow")]
        JumboBag,
    }
    public enum Bentuk
    {
        [Description("Tablet")]
        Tablet,
        [Description("Granule")]
        Granule,
        [Description("Powder")]
        Powder,
        [Description("Briket")]
        Briket,
        [Description("Prill")]
        Prill,
        [Description("Lain-Lain")]
        LainLain,
    }
  
    public enum MessageInfoType
    {
        Success,
        Error,
        Warning,
        Info
    }
    public class EnumDescriptionConverter : IValueConverter
    {
        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Enum myEnum = (Enum)value;
                string description = GetEnumDescription(myEnum);
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
