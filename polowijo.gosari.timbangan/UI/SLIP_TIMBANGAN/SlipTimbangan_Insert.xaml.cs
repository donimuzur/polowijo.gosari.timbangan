using AutoMapper;
using polowijo.gosari.timbangan.core;
using polowijo.gosari.timbangan.dal.IServices;
using polowijo.gosari.timbangan.dal.Services;
using polowijo.gosari.timbangan.dto;
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
using System.Windows.Shapes;

namespace polowijo.gosari.timbangan.UI.SLIP_TIMBANGAN
{
    /// <summary>
    /// Interaction logic for SlipTimbangan_Insert.xaml
    /// </summary>
    public partial class SlipTimbangan_Insert : Window
    {
        private ITrnSlipTimbanganServices _trnServices;
        private IMstKonsumenServices _mstKonsumenServices;
        public SlipTimbangan_Insert()
        {
            _mstKonsumenServices = new MstKonsumenServices();
            _trnServices = new TrnSlipTimbanganServices();
            InitializeComponent();
        }
        private void On_Load(object sender, RoutedEventArgs e)
        {
            dtTanggal.Text = DateTime.Now.ToString("yyyy MMM dd");
            txtJamMasuk.Text = DateTime.Now.ToString("HH:mm");
            txtNoRecord.IsReadOnly = true;
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
                var model = new SlipTimbanganModel();
                if (string.IsNullOrEmpty(dtTanggal.Text))
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
                model.TANGGAL = DateTime.Parse(dtTanggal.Text);
                model.BAHAN_BAKU = txtNamaBarang.Text;
                try
                {
                    model.BERAT_MASUK = Decimal.Parse(txtBeratMasuk.Text);
                }
                catch (Exception){}
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
                if (txtJamMasuk.SelectedTime.HasValue)
                {
                    model.JAM_MASUK = new DateTime(model.TANGGAL.Year, model.TANGGAL.Month, model.TANGGAL.Day, txtJamMasuk.SelectedTime.Value.Hour, txtJamMasuk.SelectedTime.Value.Minute, txtJamMasuk.SelectedTime.Value.Second);
                }
                if (txtJamKeluar.SelectedTime.HasValue)
                {
                    model.JAM_MASUK = new DateTime(model.TANGGAL.Year, model.TANGGAL.Month, model.TANGGAL.Day, txtJamKeluar.SelectedTime.Value.Hour, txtJamKeluar.SelectedTime.Value.Minute, txtJamKeluar.SelectedTime.Value.Second);
                }

                var Dto = Mapper.Map<TrnSlipTimbanganDto>(model);
                
                var DB =_trnServices.Save(Dto, Mapper.Map<LoginDto>(BaseController.CurrentUser));

                MstKonsumenDto KonsumenDto = new MstKonsumenDto { ALAMAT_KONSUMEN = " ", NAMA_KONSUMEN = model.PERUSAHAAN, STATUS = 1 };
                _mstKonsumenServices.InsertUpdateMstKonsumen(KonsumenDto, Mapper.Map<LoginDto>(BaseController.CurrentUser));

                KonsumenDto = new MstKonsumenDto { ALAMAT_KONSUMEN = " ", NAMA_KONSUMEN = model.SUPPLIER, STATUS = 1 };
                _mstKonsumenServices.InsertUpdateMstKonsumen(KonsumenDto, Mapper.Map<LoginDto>(BaseController.CurrentUser));
                
                MessageBox.Show(Constans.SubmitMessage.Saved, Constans.SubmitMessageTittle.Sukses, MessageBoxButton.OK, MessageBoxImage.Information);
                Btn_PrintToPDF.IsEnabled = true;
                Btn_PrintToPrinter.IsEnabled = true;
                txtID.Text = DB.ID.ToString();
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

        }

        private void Btn_PrintToPDF_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
