using polowijo.gosari.timbangan.UI.SLIP_TIMBANGAN;
using polowijo.gosari.timbangan.UI.TRN_PENGIRIMAN;
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

namespace polowijo.gosari.timbangan.UI.HOME
{
    /// <summary>
    /// Interaction logic for Home_Menu.xaml
    /// </summary>
    public partial class Home_Menu : UserControl
    {
        private UserControl _trnpengirmanView;
        private UserControl _slipTimbanganView;
        public Home_Menu()
        {
            InitializeComponent();
        }
        private void On_Load(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _slipTimbanganView = new SlipTimbangan_View();
            _trnpengirmanView = new TrnPengiriman_View();
            Mouse.OverrideCursor = null;
        }
        private void Btn_Pengiriman_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _trnpengirmanView = new TrnPengiriman_View();

            Window parentWindow = Window.GetWindow(this);
            var Content = (Grid)parentWindow.Content;

            var ContentChild1 = (Grid)Content.Children[0];
            var Header = (TextBlock)ContentChild1.Children[0];
            Header.Text = "Aplikasi Timbangan - Surat Jalan";
            var ContentChildChild1 = (StackPanel)ContentChild1.Children[1];
            var BackButton = (Button)ContentChildChild1.Children[0];
            BackButton.Visibility = Visibility.Visible;
            
            var ContentChild2 = (DockPanel)Content.Children[2];
            ContentChild2.Children.Clear();
            ContentChild2.Children.Add(_trnpengirmanView);

            
            Mouse.OverrideCursor = null;
        }

        private void Btn_SlipTimbangan_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _slipTimbanganView = new SlipTimbangan_View();

            Window parentWindow = Window.GetWindow(this);
            var Content = (Grid)parentWindow.Content;

            var ContentChild1 = (Grid)Content.Children[0];
            var Header = (TextBlock)ContentChild1.Children[0];
            Header.Text = "Aplikasi Timbangan - Slip Timbangan";
            var ContentChildChild1 = (StackPanel)ContentChild1.Children[1];
            var BackButton = (Button)ContentChildChild1.Children[0];
            BackButton.Visibility = Visibility.Visible;

            var ContentChild2 = (DockPanel)Content.Children[2];
            ContentChild2.Children.Clear();
            ContentChild2.Children.Add(_slipTimbanganView);
            Mouse.OverrideCursor = null;
        }
    }
}
