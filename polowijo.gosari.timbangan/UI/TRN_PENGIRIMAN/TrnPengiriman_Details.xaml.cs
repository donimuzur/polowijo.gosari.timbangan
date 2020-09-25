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
using System.Windows.Shapes;

namespace polowijo.gosari.timbangan.UI.TRN_PENGIRIMAN
{
    /// <summary>
    /// Interaction logic for TrnPengiriman_Details.xaml
    /// </summary>
    public partial class TrnPengiriman_Details : Window
    {
        private ITrnPengirimanServices _trnPengirimanServices;
        private ITrnDoServices _trnDoServices;
        private IMstKemasanServices _mstKemasanServices;
        private IMstBarangJadiServices _mstBarangJadiServices;
        private object IdDetails;

        public TrnPengiriman_Details(object id)
        {
            _trnPengirimanServices = new TrnPengirimanServices();
            _trnDoServices = new TrnDoServices();
            _mstKemasanServices = new MstKemasanServices();
            _mstBarangJadiServices = new MstBarangJadiServices();

            IdDetails = id;
            InitializeComponent();
        }
        private void On_Load(object sender, RoutedEventArgs e)
        {
            using (new WaitCursor())
            {
                var GetData = _trnPengirimanServices.GetById(IdDetails);
                if(GetData != null)
                {
                    //Hidden
                    txtID.Text = GetData.ID.ToString();
                    txtCreatedBy.Text = GetData.CREATED_BY;
                    txtCreatedDate.Text = GetData.CREATED_DATE.ToString("yyyy MMM dd");
                    txtStatus.Text = GetData.STATUS.ToString();
                    txtIDBarang.Text = GetData.ID_BARANG.ToString();

                    //Informasi
                    dtTanggal.Text = GetData.TANGGAL.ToString("yyyy MMM dd");
                    txtNoSuratJalan.Text = GetData.NO_SURAT_JALAN;
                    txtRit.Text = GetData.NO_RIT.ToString();
                    var Jumlah = GetData.PARTAI;
                    var TotalKirim = _trnPengirimanServices.GetAkumulasi(Convert.ToInt32(GetData.NO_DO), GetData.NO_SPB);
                    var SisaKirim = Jumlah - TotalKirim;
                    txtPartai.Text = Jumlah.ToString();
                    txtTotalKirim.Text = TotalKirim.ToString();
                    txtSisaKirim.Text = SisaKirim.ToString();
                    txtNoSpb.Text = GetData.NO_SPB;
                    txtNoDo.Text = GetData.NO_DO.ToString();

                    //Data Transportir
                    txtTrnsptNamaPt.Text = GetData.TRNSPT_NAMA_PT;
                    txtTrnsptJenisKendaraan.Text= GetData.TRNSPT_JENIS_KENDARAAN;
                    txtTrnsptNoPolisi.Text = GetData.TRNSPT_NO_POLISI;
                    txtTrsnptNamaSopir.Text = GetData.TRNSPT_NAMA_SOPIR;
                    txtTrnsptJamBrngkt.Text = GetData.TRNSPT_BERANGKAT.Value.ToString("HH:mm");

                    //Alamat Pengiriman
                    txtNamaKonsumen.Text = GetData.NAMA_KONSUMEN;
                    txtAlamatPengiriman.Text = GetData.ALAMAT_KONSUMEN;
                    txtProvinsi.Text = GetData.PROVINSI;
                    txtMadya.Text = GetData.KABUPATEN;

                    //Data Barang
                    txtNamaBarang.Text = GetData.NAMA_BARANG;
                    txtKemasan.Text = GetData.KEMASAN;
                    txtZak.Text = GetData.ZAK.ToString();
                    txtKwantum.Text = GetData.KUANTUM.ToString();
                    txtTimbangan.Text = GetData.TIMBANGAN.ToString();

                    txtKeterangan.Text = GetData.KETERANGAN;
                }
            }
        }
        private void CloseWin()
        {
            DialogResult = true;
            this.Close();
        }
        private void Btn_Simpan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (new WaitCursor())
                {
                    var model = new TrnPengirimanModel();
                    if (!dtTanggal.SelectedDate.HasValue)
                    {
                        MessageBox.Show("Tanggal Harus Di isi", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        dtTanggal.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNoSpb.Text))
                    {
                        MessageBox.Show("No SPB tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtNoSpb.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtNoDo.Text))
                    {
                        MessageBox.Show("No Polisi tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtNoDo.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtTrnsptNamaPt.Text))
                    {
                        MessageBox.Show("Nama PT tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtTrnsptNamaPt.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtTrnsptNamaPt.Text))
                    {
                        MessageBox.Show("Nama PT tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtTrnsptNamaPt.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtTrnsptJenisKendaraan.Text))
                    {
                        MessageBox.Show("Jenis Kendaraan tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtTrnsptJenisKendaraan.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtTrnsptNoPolisi.Text))
                    {
                        MessageBox.Show("No Polisi tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtTrnsptNoPolisi.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtTrsnptNamaSopir.Text))
                    {
                        MessageBox.Show("Nama Sopir tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtTrsnptNamaSopir.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNamaKonsumen.Text))
                    {
                        MessageBox.Show("Nama Konsumen tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtNamaKonsumen.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtAlamatPengiriman.Text))
                    {
                        MessageBox.Show("Alamat Kirim tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtAlamatPengiriman.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNamaBarang.Text))
                    {
                        MessageBox.Show("Nama Barang tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtNamaBarang.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtZak.Text))
                    {
                        MessageBox.Show("Zak tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtZak.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtKwantum.Text))
                    {
                        MessageBox.Show("Kwantum tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtKwantum.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtTimbangan.Text))
                    {
                        MessageBox.Show("TScale tidak boleh kosong", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        txtTimbangan.Focus();
                        return;
                    }


                    if(!string.IsNullOrEmpty(txtID.Text))
                    {
                        model.ID = Int32.Parse(txtID.Text);
                    }
                    if(!string.IsNullOrEmpty(txtCreatedBy.Text))
                    {
                        model.CREATED_BY = txtCreatedBy.Text;
                    }
                    if (!string.IsNullOrEmpty(txtCreatedDate.Text))
                    {
                        model.CREATED_DATE = Convert.ToDateTime(txtCreatedDate.Text);
                    }
                    if (!string.IsNullOrEmpty(txtStatus.Text))
                    {
                        model.STATUS =Convert.ToByte(txtStatus.Text);
                    }
                    if (!string.IsNullOrEmpty(txtIDBarang.Text))
                    {
                        model.ID_BARANG = Convert.ToInt32(txtIDBarang.Text);
                    }
                    model.TANGGAL = dtTanggal.SelectedDate.Value;
                    model.NO_SURAT_JALAN = txtNoSuratJalan.Text;
                    if(!string.IsNullOrEmpty(txtRit.Text))
                    {
                        model.NO_RIT = Int32.Parse(txtRit.Text);
                    }
                    try
                    {
                        model.PARTAI = decimal.Parse(txtPartai.Text);
                    }
                    catch (Exception) { }
                    try
                    {
                        model.TOTAL_KIRIM = decimal.Parse(txtTotalKirim.Text);
                    }
                    catch (Exception) { }
                    try
                    {
                        model.SISA_KIRIM = decimal.Parse(txtSisaKirim.Text);
                    }
                    catch (Exception) { }
                    model.NO_SPB = txtNoSpb.Text;
                    model.NO_DO = Int32.Parse(txtNoDo.Text);

                    //Data Trasportir
                    model.TRNSPT_NAMA_PT = txtTrnsptNamaPt.Text;
                    model.TRNSPT_JENIS_KENDARAAN = txtTrnsptJenisKendaraan.Text;
                    model.TRNSPT_NO_POLISI = txtTrnsptNoPolisi.Text;
                    model.TRNSPT_NAMA_SOPIR = txtTrsnptNamaSopir.Text;
                    if (txtTrnsptJamBrngkt.SelectedTime.HasValue)
                    {
                        model.TRNSPT_BERANGKAT = new DateTime(model.TANGGAL.Year, model.TANGGAL.Month, model.TANGGAL.Day, txtTrnsptJamBrngkt.SelectedTime.Value.Hour, txtTrnsptJamBrngkt.SelectedTime.Value.Minute, txtTrnsptJamBrngkt.SelectedTime.Value.Second);
                    }

                    //Alamat Pengiriman
                    model.NAMA_KONSUMEN = txtNamaKonsumen.Text;
                    model.PROVINSI = txtProvinsi.Text;
                    model.KABUPATEN = txtMadya.Text;
                    model.ALAMAT_KONSUMEN = txtAlamatPengiriman.Text;

                    //Data Barang
                    model.NAMA_BARANG = txtNamaBarang.Text;
                    model.KEMASAN = txtKemasan.Text;
                    try
                    {
                        model.ZAK = int.Parse(txtZak.Text);
                    }
                    catch (Exception) { }
                    try
                    {
                        model.KUANTUM = decimal.Parse(txtKwantum.Text);
                    }
                    catch (Exception) { }
                    try
                    {
                        model.TIMBANGAN = decimal.Parse(txtTimbangan.Text);
                    }
                    catch (Exception) { }

                    model.KETERANGAN = txtKeterangan.Text;


                    model.CREATED_BY = BaseController.CurrentUser.USERNAME;
                    model.CREATED_DATE = DateTime.Now;
                    model.STATUS = (int)StatusDocument.Open;

                    model.NO_SPB = model.NO_SPB.ToUpper();
                    model.NO_SPB = model.NO_SPB.TrimEnd('\r', '\n', ' ');
                    model.NO_SPB = model.NO_SPB.TrimStart('\r', '\n', ' ');

                    var GetBarang = _mstBarangJadiServices.GetByNama(model.NAMA_BARANG);
                    if (GetBarang != null)
                    {
                        model.ID_BARANG = GetBarang.ID;
                        //model.STOCK_AWAL = GetBarang.STOCK;
                        //model.STOCK_AKHIR = model.STOCK_AWAL - model.KUANTUM;
                        //if(model.STOCK_AKHIR < 0)
                        //{
                        //    AddMessageInfo("Stock barang tidak mencukupi", Enums.MessageInfoType.Error);
                        //    return View(Init(model));
                        //}
                    }

                    //if (model.KUANTUM > model.SISA_KIRIM )
                    //{
                    //    AddMessageInfo("Kuantum tidak boleh melebihi sisa Kirim", Enums.MessageInfoType.Error);
                    //    return View(Init(model));
                    //}

                    //if (model.SISA_KIRIM <= 0)
                    //{
                    //    AddMessageInfo("Jumlah barang yg dikirim sudah sesuai", Enums.MessageInfoType.Error);
                    //    return View(Init(model));
                    //}

                    var CheckDataSPBExist = _trnDoServices.GetBySPB(model.NO_SPB).FirstOrDefault();
                    if (CheckDataSPBExist == null)
                    {
                        MessageBox.Show("No SPB tersebut tidak ada", Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var CheckDataDOExist = _trnDoServices.GetBySpbAndDo(model.NO_SPB, model.NO_DO.ToString());
                    if (CheckDataDOExist == null)
                    {
                        MessageBox.Show("No Do tersebut tidak ada", Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var Dto = _trnPengirimanServices.Save(Mapper.Map<TrnPengirimanDto>(model), Mapper.Map<LoginDto>(BaseController.CurrentUser));
                    //_mstBarangJadiBLL.KurangSaldo(model.ID_BARANG, model.KUANTUM.Value);
                    CloseWin();
                }

                MessageBox.Show(Constans.SubmitMessage.Saved, Constans.SubmitMessageTittle.Sukses, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Batal_Click(object sender, RoutedEventArgs e)
        {
            CloseWin();
        }


        private void Btn_PrintToPrinter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

                    var Id = 0;
                    try
                    {
                        Id = Int32.Parse(txtID.Text);
                    }
                    catch (Exception) { }

                    cryRpt.Load(SystemPath + "\\Reports\\ReportSuratJalan3.rpt");
                    cryRpt.SetDatabaseLogon(SqlConnection.UserID, SqlConnection.Password, SqlConnection.DataSource, SqlConnection.InitialCatalog);
                    cryRpt.SetParameterValue("id", Id);

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

        private void Btn_PrintToPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                        var Id = 0;
                        try
                        {
                            Id = Int32.Parse(txtID.Text);
                        }
                        catch (Exception) { }

                        cryRpt.Load(SystemPath + "\\Reports\\ReportSuratJalan3_Layout.rpt");
                        cryRpt.SetDatabaseLogon(SqlConnection.UserID, SqlConnection.Password, SqlConnection.DataSource, SqlConnection.InitialCatalog);
                        cryRpt.SetParameterValue("id", Id);
                        cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fileToSave);

                    }
                    MessageBox.Show(Constans.SubmitMessage.Saved, Constans.SubmitMessageTittle.Sukses, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void txtNoDo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelper.IsValidInteger(((TextBox)sender).Text + e.Text);
        }
        private void txtNoDo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    using (new WaitCursor())
                    {
                        if (string.IsNullOrEmpty(txtNoSpb.Text) || string.IsNullOrEmpty(txtNoDo.Text))
                        {
                            return;
                        }

                        var GetData = _trnDoServices.GetBySpbAndDo(txtNoSpb.Text, txtNoDo.Text);
                        if (GetData != null)
                        {
                            txtNamaBarang.Text = GetData.NAMA_BARANG;
                            txtKemasan.Text = GetData.KEMASAN;
                            var Jumlah = GetData.JUMLAH;
                            var TotalKirim = _trnPengirimanServices.GetAkumulasi(Convert.ToInt32(GetData.NO_DO), GetData.NO_SPB);
                            var SisaKirim = Jumlah - TotalKirim;
                            txtPartai.Text = Jumlah.ToString();
                            txtTotalKirim.Text = TotalKirim.ToString();
                            txtSisaKirim.Text = SisaKirim.ToString();
                            txtAlamatPengiriman.Text = GetData.ALAMAT_KONSUMEN;
                            txtNamaKonsumen.Text = GetData.NAMA_KONSUMEN;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private decimal HitungAkumulasi(string Kemasan, string Zak)
        {
            try
            {
                using (new WaitCursor())
                {
                    decimal HitungKuantum = 0;
                    var data = new MstKemasanDto();
                    if (!string.IsNullOrEmpty(Kemasan) && !string.IsNullOrEmpty(Zak))
                    {

                        data = _mstKemasanServices.GetByNama(Kemasan);
                        HitungKuantum = data.CONVERTION.Value * Convert.ToInt32(Zak);

                        return HitungKuantum;
                    }
                    return 0;
                }
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                return 0;
            }
        }
        private void txtZak_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal result = 0;
                result = HitungAkumulasi(txtKemasan.Text, txtZak.Text);
                txtKwantum.Text = result.ToString();
            }
            catch (Exception exp)
            {
                LogError.WriteError(exp);
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessageTittle.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void txtZak_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelper.IsValidInteger(((TextBox)sender).Text + e.Text);
        }

        private void txtZak_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtZak_LostFocus(sender, e);
            }
        }

        private void txtTimbangan_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !FunctionHelper.IsValidDecimal(((TextBox)sender).Text + e.Text);
        }
    }
}
