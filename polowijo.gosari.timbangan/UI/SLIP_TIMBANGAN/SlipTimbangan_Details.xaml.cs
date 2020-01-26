using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Win32;
using polowijo.gosari.timbangan.core;
using polowijo.gosari.timbangan.dal.IServices;
using polowijo.gosari.timbangan.dal.Services;
using polowijo.gosari.timbangan.dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Shapes;
using WpfControls.Editors;

namespace polowijo.gosari.timbangan.UI.SLIP_TIMBANGAN
{
    /// <summary>
    /// Interaction logic for SlipTimbangan_Details.xaml
    /// </summary>
    public partial class SlipTimbangan_Details : Window
    {
        private ITrnSlipTimbanganServices _trnServices;
        private IMstKonsumenServices _mstKonsumenServices;
        private object IdDetails;
        public SlipTimbangan_Details(object Id)
        {
            _mstKonsumenServices = new MstKonsumenServices();
            _trnServices = new TrnSlipTimbanganServices();
            InitializeComponent();

            IdDetails = Id;
        }
        private void On_Load(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            var Data = _trnServices.GetById(this.IdDetails);
            
            if(Data != null)
            {
                txtID.Text = IdDetails.ToString();
                dtTanggal.Text = Data.TANGGAL.ToString("yyyy MMM dd");
                txtNoRecord.Text = Data.NO_RECORD.HasValue ? Data.NO_RECORD.Value.ToString() : "";
                txtSuratJalan.Text = Data.NO_SURAT_JALAN;
                txtPerusahaan.Text = Data.PERUSAHAAN;
                txtNamaSupplier.Text = Data.SUPPLIER;
                txtPolisi.Text = Data.NO_POLISI;
                txtPengemudi.Text = Data.PENGEMUDI;
                txtNamaBarang.Text = Data.BAHAN_BAKU;
                txtJamMasuk.Text = Data.JAM_MASUK.HasValue ? Data.JAM_MASUK.Value.ToString("HH:mm") : "";
                txtJamKeluar.Text = Data.JAM_KELUAR.HasValue ? Data.JAM_KELUAR.Value.ToString("HH:mm") : "";

                txtBeratMasuk.Text = Data.BERAT_MASUK.HasValue ? Data.BERAT_MASUK.Value.ToString() : "";
                txtBeratKeluar.Text = Data.BERAT_KELUAR.HasValue ? Data.BERAT_KELUAR.Value.ToString() : "";
                txtBeratBrutto.Text = Data.BERAT_BRUTTO.HasValue ? Data.BERAT_BRUTTO.Value.ToString() : "";
                txtBeratNetto.Text = Data.BERAT_NETTO.HasValue ? Data.BERAT_NETTO.Value.ToString() : "";

                txtKeterangan.Text = Data.KETERANGAN;
            }
            txtNoRecord.IsReadOnly = true;
            Mouse.OverrideCursor = null;
        }
        private void CloseWin()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            DialogResult = true;
            this.Close();
            Mouse.OverrideCursor = null;
        }
        private void Btn_Simpan_Click(object sender, RoutedEventArgs e)
        {
          
            try
            {
                var model = new SlipTimbanganModel();
                if (dtTanggal.SelectedDate.HasValue)
                {
                    MessageBox.Show("Tanggal Harus Di isi", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    dtTanggal.Focus();

                    return;
                }

                if (string.IsNullOrEmpty(txtPerusahaan.Text))
                {
                    MessageBox.Show("Perusahaan tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtPerusahaan.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtPolisi.Text))
                {
                    MessageBox.Show("No Polisi tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtPolisi.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtNamaBarang.Text))
                {
                    MessageBox.Show("Barang tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtNamaBarang.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtBeratMasuk.Text))
                {
                    MessageBox.Show("Berat Masuk tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtBeratMasuk.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtJamMasuk.Text))
                {
                    MessageBox.Show("Jam Masuk tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtJamMasuk.Focus();
                    return;
                }
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                model.TANGGAL = DateTime.Parse(dtTanggal.Text);
                model.BAHAN_BAKU = txtNamaBarang.Text;
                try
                {
                    model.BERAT_MASUK = Decimal.Parse(txtBeratMasuk.Text);
                }
                catch (Exception) { }
                try
                {
                    model.BERAT_KELUAR = Decimal.Parse(txtBeratKeluar.Text);
                }
                catch (Exception) { }
                try
                {
                    model.BERAT_BRUTTO = Decimal.Parse(txtBeratBrutto.Text);
                }
                catch (Exception) { }
                try
                {
                    model.BERAT_NETTO = Decimal.Parse(txtBeratNetto.Text);
                }
                catch (Exception) { }
                model.CREATED_BY = BaseController.CurrentUser.USERNAME;
                model.CREATED_DATE = DateTime.Now;
                model.KETERANGAN = txtKeterangan.Text;
                model.NO_POLISI = txtPolisi.Text;
                model.PENGEMUDI = txtPengemudi.Text;
                model.PERUSAHAAN = txtPerusahaan.Text;
                model.SUPPLIER = txtNamaSupplier.Text;
                model.STATUS = (int)StatusDocument.Open;

                if(txtJamMasuk.SelectedTime.HasValue)
                {
                    model.JAM_MASUK = new DateTime(model.TANGGAL.Year, model.TANGGAL.Month, model.TANGGAL.Day, txtJamMasuk.SelectedTime.Value.Hour, txtJamMasuk.SelectedTime.Value.Minute, txtJamMasuk.SelectedTime.Value.Second);
                }
                if (txtJamKeluar.SelectedTime.HasValue)
                {
                    model.JAM_MASUK = new DateTime(model.TANGGAL.Year, model.TANGGAL.Month, model.TANGGAL.Day, txtJamKeluar.SelectedTime.Value.Hour, txtJamKeluar.SelectedTime.Value.Minute, txtJamKeluar.SelectedTime.Value.Second);
                }
                var Dto = Mapper.Map<TrnSlipTimbanganDto>(model);


                var DB = _trnServices.Save(Dto, Mapper.Map<LoginDto>(BaseController.CurrentUser));

                MstKonsumenDto KonsumenDto = new MstKonsumenDto { ALAMAT_KONSUMEN = " ", NAMA_KONSUMEN = model.PERUSAHAAN, STATUS = 1 };
                _mstKonsumenServices.InsertUpdateMstKonsumen(KonsumenDto, Mapper.Map<LoginDto>(BaseController.CurrentUser));

                KonsumenDto = new MstKonsumenDto { ALAMAT_KONSUMEN = " ", NAMA_KONSUMEN = model.SUPPLIER, STATUS = 1 };
                _mstKonsumenServices.InsertUpdateMstKonsumen(KonsumenDto, Mapper.Map<LoginDto>(BaseController.CurrentUser));

                MessageBox.Show(Constans.SubmitMessage.Saved, Constans.SubmitMessageTittle.Sukses, MessageBoxButton.OK, MessageBoxImage.Information);
                Btn_PrintToPDF.IsEnabled = true;
                Btn_PrintToPrinter.IsEnabled = true;
                txtID.Text = DB.ID.ToString();
                Mouse.OverrideCursor = null;
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                Mouse.OverrideCursor = null;
            }
        }

        private void Btn_Batal_Click(object sender, RoutedEventArgs e)
        {
            CloseWin();
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void txtBeratMasuk_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelper.IsValidDecimal(((TextBox)sender).Text + e.Text);
        }

        private void txtBeratKeluar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelper.IsValidDecimal(((TextBox)sender).Text + e.Text);
        }

        private void txtBeratBrutto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelper.IsValidDecimal(((TextBox)sender).Text + e.Text);
        }

        private void txtBeratNetto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelper.IsValidDecimal(((TextBox)sender).Text + e.Text);
        }

        private void Btn_PrintToPrinter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                var connectString = ConfigurationManager.ConnectionStrings["TIMBANGANEntities"].ConnectionString;
                var entityStringBuilder = new EntityConnectionStringBuilder(connectString);
                SqlConnectionStringBuilder SqlConnection = new SqlConnectionStringBuilder(entityStringBuilder.ProviderConnectionString);
                var PrinterName = ConfigurationManager.AppSettings["PrinterName"];

                ReportDocument cryRpt = new ReportDocument();

                // this will be in a temp directory.
                string SystemPath = System.AppDomain.CurrentDomain.BaseDirectory;
                //once you have the path you get the directory with:
                var directory = System.IO.Path.GetDirectoryName(SystemPath);

                cryRpt.Load(SystemPath + "\\Reports\\ReportSlipTimbangan.rpt");
                cryRpt.SetDatabaseLogon(SqlConnection.UserID, SqlConnection.Password, SqlConnection.DataSource, SqlConnection.InitialCatalog);
                cryRpt.SetParameterValue("id", IdDetails);

                //cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, fullPath);

                System.Drawing.Printing.PrinterSettings printersettings = new System.Drawing.Printing.PrinterSettings();
                System.Drawing.Printing.PageSettings pageSettings = new System.Drawing.Printing.PageSettings();
                var paper = new System.Drawing.Printing.PaperSize("Custom", 300, 600);

                printersettings.DefaultPageSettings.Landscape = false;
                printersettings.DefaultPageSettings.PaperSize = paper;

                pageSettings.PaperSize = paper;
                pageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                pageSettings.Landscape = false;

                var nama = System.Drawing.Printing.PrinterSettings.InstalledPrinters;

                printersettings.PrinterName = PrinterName;

                cryRpt.PrintToPrinter(printersettings, pageSettings, false);
                MessageBox.Show("File Printing", Constans.SubmitMessageTittle.Sukses, MessageBoxButton.OK, MessageBoxImage.Information);
                Mouse.OverrideCursor =null;
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_PrintToPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = "pdf";
                openFileDialog.Filter = "PDF|*.pdf";
                openFileDialog.Title = "Save Path";
                string fileToSave;
                if (openFileDialog.ShowDialog() == true)
                {
                    fileToSave = openFileDialog.FileName;
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
                    cryRpt.Load(SystemPath + "\\Reports\\ReportSlipTimbangan.rpt");
                    cryRpt.SetDatabaseLogon(SqlConnection.UserID, SqlConnection.Password, SqlConnection.DataSource, SqlConnection.InitialCatalog);
                    cryRpt.SetParameterValue("id", IdDetails);
                    cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fileToSave);

                    MessageBox.Show(Constans.SubmitMessage.Saved, Constans.SubmitMessageTittle.Sukses, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Mouse.OverrideCursor = null;
            }
            catch (Exception exp)
            {
                Mouse.OverrideCursor = null;
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
