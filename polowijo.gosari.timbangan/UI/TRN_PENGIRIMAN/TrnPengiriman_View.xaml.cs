using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using polowijo.gosari.timbangan.core;
using polowijo.gosari.timbangan.dal.IServices;
using polowijo.gosari.timbangan.dal.Services;
using polowijo.gosari.timbangan.dto;
using polowijo.gosari.timbangan.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
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
        private ITrnPengirimanServices _trnPengirimanServices;
        private Window _insertWindow;
        private Window _detailsWindow;

        private ICollectionView _data;

        private IValueConverter _converter;
        Dictionary<string, Predicate<TrnPengirimanModel>> filters = new Dictionary<string, Predicate<TrnPengirimanModel>>();
        public TrnPengiriman_View()
        {
            _trnPengirimanServices = new TrnPengirimanServices();
            _converter = new EnumDescriptionConverter();

            _insertWindow = new TrnPengiriman_Insert();
            _detailsWindow = new TrnPengiriman_Details(0);
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
            DataGridTextColumn NO_SPB = new DataGridTextColumn();
            DataGridTextColumn NO_DO_RIT = new DataGridTextColumn();
            DataGridTextColumn TANGGAL = new DataGridTextColumn();
            DataGridTextColumn NAMA_KONSUMEN = new DataGridTextColumn();
            DataGridTextColumn NAMA_BARANG = new DataGridTextColumn();
            DataGridTextColumn KUANTUM = new DataGridTextColumn();
            DataGridTextColumn NO_SURAT_JALAN = new DataGridTextColumn();
            DataGridTextColumn DUMMY = new DataGridTextColumn();

            Binding TANGGALBinding = new Binding("TANGGAL");
            TANGGALBinding.StringFormat = "dd MMM yyyy";
            
            Binding KUANTUMBinding = new Binding("KUANTUM");
            KUANTUMBinding.StringFormat = "{0:N0}";

            ID.Binding = new Binding("ID");
            NO_SPB.Binding = new Binding("NO_SPB");
            NO_DO_RIT.Binding = new Binding("NO_DO_RIT");
            TANGGAL.Binding = TANGGALBinding;
            NAMA_KONSUMEN.Binding = new Binding("NAMA_KONSUMEN");
            NAMA_BARANG.Binding = new Binding("NAMA_BARANG");
            KUANTUM.Binding = KUANTUMBinding;
            NO_SURAT_JALAN.Binding = new Binding("NO_SURAT_JALAN");

            ID.Header = "ID";
            NO_SPB.Header = "No SPB";
            NO_DO_RIT.Header = "No DO/Rit";
            TANGGAL.Header = "Tanggal";
            NAMA_KONSUMEN.Header = "Konsumen";
            NAMA_BARANG.Header = "Produk";
            KUANTUM.Header = "Kuantum (Kg)";
            NO_SURAT_JALAN.Header = "No Surat Jalan";

            Dgv_Home.Columns.Add(TANGGAL);
            Dgv_Home.Columns.Add(NO_SPB);
            Dgv_Home.Columns.Add(NO_DO_RIT);
            Dgv_Home.Columns.Add(NO_SURAT_JALAN);
            Dgv_Home.Columns.Add(NAMA_KONSUMEN);
            Dgv_Home.Columns.Add(NAMA_BARANG);
            Dgv_Home.Columns.Add(KUANTUM);

            //TIDAK DI TAMPILKAN
            Dgv_Home.Columns.Add(ID);
            Dgv_Home.Columns.Add(DUMMY);

            DUMMY.Visibility = Visibility.Hidden;
            ID.Visibility = Visibility.Hidden;

            #endregion

            PopulateData();
        }
        private void PopulateData()
        {
            try
            {
                var Data = Mapper.Map<List<TrnPengirimanModel>>(_trnPengirimanServices.GetActiveAll().OrderByDescending(x=>x.TANGGAL));
                Data.ForEach(x =>
                {
                    x.NO_DO_RIT = x.NO_DO.ToString().PadLeft(4,'0') + "/" + x.NO_RIT;
                });
                _data = CollectionViewSource.GetDefaultView(Data);
                _data.Filter = new Predicate<object>(FilterCandidates);

                Dgv_Home.ItemsSource = _data;
            }
            catch (Exception exp)
            {
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.WriteError(exp);
                return;
            }
        }
        private bool FilterCandidates(object obj)
        {
            TrnPengirimanModel c = (TrnPengirimanModel)obj;
            return filters.Values
                .Aggregate(true,
                    (prevValue, predicate) => prevValue && predicate(c));
        }
        private void AddFilterAndRefresh(string name, Predicate<TrnPengirimanModel> predicate)
        {
            filters.Add(name, predicate);
            _data.Refresh();
        }
        private void Tambah_Click(object sender, RoutedEventArgs e)
        {
            _insertWindow = new TrnPengiriman_Insert();
            var result = _insertWindow.ShowDialog();
            if (result == true)
            {
                PopulateData();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var idx = (TrnPengirimanModel)Dgv_Home.SelectedItem;
            if (idx == null)
                MessageBox.Show("Tidak ada data yg dipilih", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                _detailsWindow = new TrnPengiriman_Details(idx.ID);
                var result = _detailsWindow.ShowDialog();
                if (result == true)
                {
                    PopulateData();
                }
            }
        }

        private void Hapus_Click(object sender, RoutedEventArgs e)
        {
            var idx = (TrnPengirimanModel)Dgv_Home.SelectedItem;
            if (idx == null)
                MessageBox.Show("Tidak ada data yg dipilih", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                var Result = MessageBox.Show("Apakah and ingin menghapus data ini ?", "Info!", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (Result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _trnPengirimanServices.DeleteById(idx.ID, Mapper.Map<LoginDto>(BaseController.CurrentUser));
                        MessageBox.Show("Sukses Hapus Data", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                        PopulateData();
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Error Hapus Data", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        LogError.WriteError(exp);
                        return;
                    }
                }
            }
        }

        private void Dgv_Home_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var idx = (TrnPengirimanModel)Dgv_Home.SelectedItem;

            if (idx == null)
            {
                return;
            }

            _detailsWindow = new TrnPengiriman_Details(idx.ID);
            var result = _detailsWindow.ShowDialog();
            if (result == true)
            {
                PopulateData();
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filters.Clear();
                _data.Refresh();

                if (!string.IsNullOrEmpty(Filter_Tanggal.Text))
                {
                    AddFilterAndRefresh("TANGGAL", candidate => Convert.ToDateTime(Filter_Tanggal.Text).Day == candidate.TANGGAL.Day && Convert.ToDateTime(Filter_Tanggal.Text).Month == candidate.TANGGAL.Month && Convert.ToDateTime(Filter_Tanggal.Text).Year == candidate.TANGGAL.Year);
                }

                if (!string.IsNullOrEmpty(Filter_NoSpb.Text))
                {
                    AddFilterAndRefresh("NO_SPB", candidate => Filter_NoSpb.Text.ToUpper() == candidate.NO_SPB.ToUpper());
                }

                if (!string.IsNullOrEmpty(Filter_NoDo.Text))
                {
                    AddFilterAndRefresh("NO_DO", candidate => Filter_NoDo.Text.ToUpper() == candidate.NO_DO.ToString().ToUpper());
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.WriteError(exp);
                return;
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Filter_NoSpb.Text = "";
            Filter_NoDo.Text = "";
            Filter_Tanggal.Text = "";

            filters.Clear();
            _data.Refresh();
        }

        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnFilter_Click(sender, e);
            }
        }

        private void Filter_NoDo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelper.IsValidInteger(((TextBox)sender).Text + e.Text);
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var idx = (TrnPengirimanModel)Dgv_Home.SelectedItem;

                if (idx == null)
                {
                    MessageBox.Show("Tidak ada data yg dipilih", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (new WaitCursor())
                {
                    var connectString = ConfigurationManager.ConnectionStrings["TIMBANGANEntities"].ConnectionString;
                    var entityStringBuilder = new EntityConnectionStringBuilder(connectString);
                    SqlConnectionStringBuilder SqlConnection = new SqlConnectionStringBuilder(entityStringBuilder.ProviderConnectionString);
                    var PrinterName = ConfigurationManager.AppSettings["PrinterName"];

                    ReportDocument cryRpt = new ReportDocument();

                    // this will be in a temp directory.
                    string SystemPath = System.AppDomain.CurrentDomain.BaseDirectory;
                    //once you have the path you get the directory with:
                    var directory = System.IO.Path.GetDirectoryName(SystemPath);
                    

                    cryRpt.Load(SystemPath + "\\Reports\\ReportSuratJalan3.rpt");
                    cryRpt.SetDatabaseLogon(SqlConnection.UserID, SqlConnection.Password, SqlConnection.DataSource, SqlConnection.InitialCatalog);
                    cryRpt.SetParameterValue("id", idx.ID);

                    //cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, fullPath);

                    System.Drawing.Printing.PrinterSettings printersettings = new System.Drawing.Printing.PrinterSettings();
                    System.Drawing.Printing.PageSettings pageSettings = new System.Drawing.Printing.PageSettings();
                    var paper = new System.Drawing.Printing.PaperSize("Custom", 850, 650);

                    printersettings.DefaultPageSettings.Landscape = false;
                    printersettings.DefaultPageSettings.PaperSize = paper;

                    pageSettings.PaperSize = paper;
                    pageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                    pageSettings.Landscape = false;

                    var nama = System.Drawing.Printing.PrinterSettings.InstalledPrinters;

                    printersettings.PrinterName = PrinterName;

                    cryRpt.PrintToPrinter(printersettings, pageSettings, false);
                }

                MessageBox.Show("File Printing", Constans.SubmitMessageTittle.Sukses, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    public class TrnSpbSuggestionProvider : ISuggestionProvider
    {
        private ITrnDoServices _trnDoServices;
        public TrnSpbSuggestionProvider()
        {
            _trnDoServices = new TrnDoServices();
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
                var Data = _trnDoServices.GetAll();
                lst1.AddRange(Data.Where(x => x.NO_SPB.ToUpper().Contains(filter.ToUpper())).Select(x => x.NO_SPB.ToUpper()).Distinct());
                return lst1;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    public class MstKonsumenSuggestionProvider : ISuggestionProvider
    {
        private IMstKonsumenServices _mtsKonsumenServices;
        public MstKonsumenSuggestionProvider()
        {
            _mtsKonsumenServices = new MstKonsumenServices();
        }
        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter))
                {
                    return null;
                }
                List<string> lst2 = new List<string>();
                var Data = _mtsKonsumenServices.GetAll();
                lst2.AddRange(Data.Where(x => x.NAMA_KONSUMEN.ToUpper().Contains(filter.ToUpper())).Select(x => x.NAMA_KONSUMEN.ToUpper()).Distinct());
                return lst2;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class ProvinsiSuggestionProvider : ISuggestionProvider
    {
        public ProvinsiSuggestionProvider()
        {
        }
        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter))
                {
                    return null;
                }
                List<string> lst2 = new List<string>();
                string SystemPath = System.AppDomain.CurrentDomain.BaseDirectory;
                //once you have the path you get the directory with:
                var directory = System.IO.Path.GetDirectoryName(SystemPath);
                var Data = FunctionHelper.LoadJson(directory + "\\Json\\provinsi.json");
                lst2.AddRange(Data.Where(x => x.data.ToUpper().Contains(filter.ToUpper())).Select(x => x.data.ToUpper()).Distinct());
                return lst2;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class JenisKendaraanSuggestionProvider : ISuggestionProvider
    {
        public JenisKendaraanSuggestionProvider()
        {
        }
        public System.Collections.IEnumerable GetSuggestions(string filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter))
                {
                    return null;
                }
                List<string> lst2 = new List<string>();
                string SystemPath = System.AppDomain.CurrentDomain.BaseDirectory;
                //once you have the path you get the directory with:
                var directory = System.IO.Path.GetDirectoryName(SystemPath);
                var Data = FunctionHelper.LoadJson(directory + "\\Json\\JenisKendaraan.json");
                lst2.AddRange(Data.Where(x => x.data.ToUpper().Contains(filter.ToUpper())).Select(x => x.data.ToUpper()).Distinct());
                return lst2;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
