using polowijo.gosari.timbangan.dal.IServices;
using polowijo.gosari.timbangan.dal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControls.Editors;

namespace polowijo.gosari.timbangan.UI.TRN_PENGIRIMAN
{
    /// <summary>
    /// Interaction logic for TrnPengiriman_View.xaml
    /// </summary>
    public partial class TrnPengiriman_View : UserControl
    {
        public TrnPengiriman_View()
        {
            InitializeComponent();
        }
        private void On_Load(object sender, RoutedEventArgs e)
        {
            Init();
            Filter_Tanggal.Text = DateTime.Now.ToString("dd MMM yyyy");
            BtnFilter_Click(sender, e);
        }
        private void Init()
        {
            #region --- DGV Main ---
            Dgv_Home.CanUserAddRows = false;
            Dgv_Home.CanUserDeleteRows = false;
            Dgv_Home.IsReadOnly = true;
            Dgv_Home.AutoGenerateColumns = false;

            DataGridTextColumn ID = new DataGridTextColumn();
            DataGridTextColumn NO_SURAT_JALAN = new DataGridTextColumn();
            DataGridTextColumn NO_RECORD = new DataGridTextColumn();
            DataGridTextColumn TANGGAL = new DataGridTextColumn();
            DataGridTextColumn NO_POLISI = new DataGridTextColumn();
            DataGridTextColumn PERUSAHAAN = new DataGridTextColumn();
            DataGridTextColumn SUPPLIER = new DataGridTextColumn();
            DataGridTextColumn PENGEMUDI = new DataGridTextColumn();
            DataGridTextColumn BERAT_MASUK = new DataGridTextColumn();
            DataGridTextColumn BERAT_KELUAR = new DataGridTextColumn();
            DataGridTextColumn BERAT_BRUTTO = new DataGridTextColumn();
            DataGridTextColumn BERAT_NETTO = new DataGridTextColumn();
            DataGridTextColumn JAM_MASUK = new DataGridTextColumn();
            DataGridTextColumn JAM_KELUAR = new DataGridTextColumn();
            DataGridTextColumn BAHAN_BAKU = new DataGridTextColumn();
            DataGridTextColumn KETERANGAN = new DataGridTextColumn();
            DataGridTextColumn STATUS = new DataGridTextColumn();
            DataGridTextColumn CREATED_BY = new DataGridTextColumn();
            DataGridTextColumn CREATED_DATE = new DataGridTextColumn();
            DataGridTextColumn MODIFIED_BY = new DataGridTextColumn();
            DataGridTextColumn MODIFIED_DATE = new DataGridTextColumn();
            DataGridTextColumn REMARKS = new DataGridTextColumn();
            DataGridTextColumn DUMMY = new DataGridTextColumn();

            Binding TANGGALBinding = new Binding("TANGGAL");
            TANGGALBinding.StringFormat = "dd MMM yyyy";

            Binding CREATED_DATEBinding = new Binding("CREATED_DATE");
            CREATED_DATEBinding.StringFormat = "dd MMM yyyy";

            Binding MODIFIED_DATEBinding = new Binding("MODIFIED_DATE");
            MODIFIED_DATEBinding.StringFormat = "dd MMM yyyy";

            Binding JAM_MASUKBinding = new Binding("JAM_MASUK");
            JAM_MASUKBinding.StringFormat = "HH:mm";

            Binding JAM_KELUARBinding = new Binding("JAM_KELUAR");
            JAM_KELUARBinding.StringFormat = "HH:mm";

            Binding BERAT_MASUKBinding = new Binding("BERAT_MASUK");
            BERAT_MASUKBinding.StringFormat = "{0:N2}";

            Binding BERAT_KELUARBinding = new Binding("BERAT_KELUAR");
            BERAT_KELUARBinding.StringFormat = "{0:N2}";

            Binding BERAT_BRUTTOBinding = new Binding("BERAT_BRUTTO");
            BERAT_BRUTTOBinding.StringFormat = "{0:N2}";

            Binding BERAT_NETTOBinding = new Binding("BERAT_NETTO");
            BERAT_NETTOBinding.StringFormat = "{0:N2}";

            ID.Binding = new Binding("ID");
            NO_SURAT_JALAN.Binding = new Binding("NO_SURAT_JALAN");
            NO_RECORD.Binding = new Binding("NO_RECORD");
            TANGGAL.Binding = TANGGALBinding;
            NO_POLISI.Binding = new Binding("NO_POLISI");
            PERUSAHAAN.Binding = new Binding("PERUSAHAAN");
            SUPPLIER.Binding = new Binding("SUPPLIER");
            PENGEMUDI.Binding = new Binding("PENGEMUDI");
            BERAT_MASUK.Binding = BERAT_MASUKBinding;
            BERAT_KELUAR.Binding = BERAT_KELUARBinding;
            BERAT_BRUTTO.Binding = BERAT_BRUTTOBinding;
            BERAT_NETTO.Binding = BERAT_NETTOBinding;
            JAM_MASUK.Binding = JAM_MASUKBinding;
            JAM_KELUAR.Binding = JAM_KELUARBinding;
            BAHAN_BAKU.Binding = new Binding("BAHAN_BAKU");
            KETERANGAN.Binding = new Binding("KETERANGAN");
            STATUS.Binding = new Binding("STATUS");
            CREATED_BY.Binding = new Binding("CREATED_BY");
            CREATED_DATE.Binding = CREATED_DATEBinding;
            MODIFIED_BY.Binding = new Binding("MODIFIED_BY");
            MODIFIED_DATE.Binding = MODIFIED_DATEBinding;
            REMARKS.Binding = new Binding("REMARKS");

            ID.Header = "ID";
            NO_SURAT_JALAN.Header = "Surat Jalan";
            NO_RECORD.Header = "No Record";
            TANGGAL.Header = "Tanggal";
            NO_POLISI.Header = "No Polisi";
            PERUSAHAAN.Header = "Perusahaan";
            SUPPLIER.Header = "Supplier";
            PENGEMUDI.Header = "Pengemudi";
            BERAT_MASUK.Header = "Berat Masuk";
            BERAT_KELUAR.Header = "Berat Keluar";
            BERAT_BRUTTO.Header = "Berat Brutto";
            BERAT_NETTO.Header = "Berat Netto";
            JAM_MASUK.Header = "Jam Masuk";
            JAM_KELUAR.Header = "Jam Keluar";
            BAHAN_BAKU.Header = "Barang";
            KETERANGAN.Header = "Keterangan";
            STATUS.Header = "Status";

            Dgv_Home.Columns.Add(TANGGAL);
            Dgv_Home.Columns.Add(NO_SURAT_JALAN);
            Dgv_Home.Columns.Add(NO_RECORD);
            Dgv_Home.Columns.Add(PERUSAHAAN);
            Dgv_Home.Columns.Add(SUPPLIER);
            Dgv_Home.Columns.Add(NO_POLISI);
            Dgv_Home.Columns.Add(BAHAN_BAKU);

            //TIDAK DI TAMPILKAN
            Dgv_Home.Columns.Add(ID);
            Dgv_Home.Columns.Add(DUMMY);

            DUMMY.Visibility = Visibility.Hidden;
            ID.Visibility = Visibility.Hidden;
            
            #endregion
        }
        private void Tambah_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Dgv_Home_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
    public class TrnSpbSuggestionProvider : ISuggestionProvider
    {
        private ITrnPengirimanServices _trnPengirimanServices;
        public TrnSpbSuggestionProvider()
        {
            _trnPengirimanServices = new TrnPengirimanServices();
        }
        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter))
                {
                    return null;
                }
                List<string> lst1 = new List<string>();
                var Data = _trnPengirimanServices.GetAll();
                lst1.AddRange(Data.Where(x => x.NO_SPB.ToUpper().Contains(filter.ToUpper())).Select(x => x.NO_SPB.ToUpper()).Distinct());
                return lst1;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
