using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Win32;
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

namespace polowijo.gosari.timbangan.UI.RPT_REALISASI
{
    /// <summary>
    /// Interaction logic for RptRealisasi_View.xaml
    /// </summary>
    public partial class RptRealisasi_View : UserControl
    {
        private IRptRealisasiHarianServices _rptHarianServices;
        private ICollectionView _data;

        Dictionary<string, Predicate<RptEkspedisiHarianModel>> filters = new Dictionary<string, Predicate<RptEkspedisiHarianModel>>();
        public RptRealisasi_View()
        {
            InitializeComponent();
        }
        private void On_Load(object sender, RoutedEventArgs e)
        {
            Init();
            Filter_Tanggal.Text = DateTime.Now.ToString("dd MMM yyyy");
        }
        private void Init()
        {
            #region --- DGV Main ---
            Dgv_Home.CanUserAddRows = false;
            Dgv_Home.CanUserDeleteRows = false;
            Dgv_Home.IsReadOnly = true;
            Dgv_Home.AutoGenerateColumns = false;
            
            DataGridTextColumn NO_SPB = new DataGridTextColumn();
            DataGridTextColumn NO_DO = new DataGridTextColumn();
            DataGridTextColumn NAMA_KONSUMEN = new DataGridTextColumn();
            DataGridTextColumn NAMA_BARANG = new DataGridTextColumn();
            DataGridTextColumn TOTAL_TRUCK = new DataGridTextColumn();
            DataGridTextColumn PARTAI = new DataGridTextColumn();
            DataGridTextColumn TOTAL_KUANTUM = new DataGridTextColumn();
            DataGridTextColumn TOTAL_TIMBANGAN = new DataGridTextColumn();
            DataGridTextColumn DUMMY = new DataGridTextColumn();

            Binding PARTAIBinding = new Binding("JUMLAH");
            PARTAIBinding.StringFormat = "{0:N0}";
            Binding TOTALKUANTUMBinding = new Binding("TOTAL");
            TOTALKUANTUMBinding.StringFormat = "{0:N0}";
            Binding TOTAL_TIMBANGANBinding = new Binding("TOTAL_TIMBANGAN");
            TOTAL_TIMBANGANBinding.StringFormat = "{0:N0}";
            
            NO_SPB.Binding = new Binding("NO_SPB");
            NO_DO.Binding = new Binding("NO_DO_NEW");
            NAMA_KONSUMEN.Binding = new Binding("NAMA_KONSUMEN");
            NAMA_BARANG.Binding = new Binding("NAMA_BARANG");
            PARTAI.Binding = PARTAIBinding;
            TOTAL_KUANTUM.Binding = TOTALKUANTUMBinding;
            TOTAL_TIMBANGAN.Binding = TOTAL_TIMBANGANBinding;
            TOTAL_TRUCK.Binding = new Binding("TOTAL_TRUCK");
            
            NO_SPB.Header = "No SPB";
            NO_DO.Header = "No DO";
            NAMA_KONSUMEN.Header = "Konsumen";
            NAMA_BARANG.Header = "Produk";
            TOTAL_TRUCK.Header = "Total Truk";
            PARTAI.Header = "Partai";
            TOTAL_KUANTUM.Header = "Total Kuantum";
            TOTAL_TIMBANGAN.Header = "Total Timbangan";

            Dgv_Home.Columns.Add(NO_SPB);
            Dgv_Home.Columns.Add(NO_DO);
            Dgv_Home.Columns.Add(NAMA_KONSUMEN);
            Dgv_Home.Columns.Add(NAMA_BARANG);
            Dgv_Home.Columns.Add(TOTAL_TRUCK);
            Dgv_Home.Columns.Add(PARTAI);
            Dgv_Home.Columns.Add(TOTAL_KUANTUM);
            Dgv_Home.Columns.Add(TOTAL_TIMBANGAN);

            //TIDAK DI TAMPILKAN
            Dgv_Home.Columns.Add(DUMMY);

            DUMMY.Visibility = Visibility.Hidden;

            #endregion
        }
        private void PopulateData(string Tanggal)
        {
            _rptHarianServices = new RptRealisasiHarianServices();
            var Data = Mapper.Map<List<RptEkspedisiHarianModel>>(_rptHarianServices.GetRpt(Tanggal));
            Data.ForEach(x =>
            {
                x.NO_DO_NEW = x.NO_DO.ToString().PadLeft(4, '0');
            });
            _data = CollectionViewSource.GetDefaultView(Data);

            Dgv_Home.ItemsSource = _data;
        }
       
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(Filter_Tanggal.Text))
            {
                PopulateData(Filter_Tanggal.Text);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {;
            Filter_Tanggal.Text = "";
            _data = CollectionViewSource.GetDefaultView(new List<RptEkspedisiHarianModel>());

            Dgv_Home.ItemsSource = _data;
        }

        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {  
                if (string.IsNullOrEmpty(Filter_Tanggal.Text))
                {
                    MessageBox.Show("Filter tidak boleh kosong", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                    var Date = Convert.ToDateTime(Filter_Tanggal.Text);

                    cryRpt.Load(SystemPath + "\\Reports\\ReportRealisasiHarian.rpt");
                    cryRpt.SetDatabaseLogon(SqlConnection.UserID, SqlConnection.Password, SqlConnection.DataSource, SqlConnection.InitialCatalog);
                    cryRpt.SetParameterValue("@Date", Date.ToString("yyyy-MM-dd"));

                    //cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, fullPath);

                    System.Drawing.Printing.PrinterSettings printersettings = new System.Drawing.Printing.PrinterSettings();
                    System.Drawing.Printing.PageSettings pageSettings = new System.Drawing.Printing.PageSettings();
                    var paper = new System.Drawing.Printing.PaperSize("A4", 650, 850 );

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
        private void PrintPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Filter_Tanggal.Text))
                {
                    MessageBox.Show("Filter tidak boleh kosong", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "pdf";
                saveFileDialog.Filter = "PDF|*.pdf";
                saveFileDialog.Title = "Save Path";
                string fileToSave;
                if (saveFileDialog.ShowDialog() == true)
                {
                    using (new WaitCursor())
                    {
                        fileToSave = saveFileDialog.FileName;
                        var connectString = ConfigurationManager.ConnectionStrings["TIMBANGANEntities"].ConnectionString;
                        var entityStringBuilder = new EntityConnectionStringBuilder(connectString);
                        SqlConnectionStringBuilder SqlConnection = new SqlConnectionStringBuilder(entityStringBuilder.ProviderConnectionString);

                        ReportDocument cryRpt = new ReportDocument();

                        // this will be in a temp directory.
                        string SystemPath = System.AppDomain.CurrentDomain.BaseDirectory;
                        //once you have the path you get the directory with:
                        var directory = System.IO.Path.GetDirectoryName(SystemPath);

                        if (System.IO.File.Exists(fileToSave))
                        {
                            System.IO.File.Delete(fileToSave);
                        }

                        var Date = Convert.ToDateTime(Filter_Tanggal.Text);

                        cryRpt.Load(SystemPath + "\\Reports\\ReportRealisasiHarian.rpt");
                        cryRpt.SetDatabaseLogon(SqlConnection.UserID, SqlConnection.Password, SqlConnection.DataSource, SqlConnection.InitialCatalog);
                        cryRpt.SetParameterValue("@Date", Date.ToString("yyyy-MM-dd"));

                        cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fileToSave);

                        MessageBox.Show(Constans.SubmitMessage.Saved, Constans.SubmitMessageTittle.Sukses, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

       
    }
}
