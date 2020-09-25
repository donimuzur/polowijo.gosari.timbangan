using polowijo.gosari.timbangan.UI.RPT_REALISASI;
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

namespace polowijo.gosari.timbangan.UI.REPORTS
{
    /// <summary>
    /// Interaction logic for Reports_Menu.xaml
    /// </summary>
    public partial class Reports_Menu : UserControl
    {
        private UserControl _rptRealisasiView;
        public Reports_Menu()
        {
            InitializeComponent();
        }
        private void On_Load(object sender, RoutedEventArgs e)
        {
            _rptRealisasiView = new RptRealisasi_View();
        }

        private void Btn_Rekap_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Btn_RptRealisasi_Click(object sender, RoutedEventArgs e)
        {
            _rptRealisasiView = new RptRealisasi_View();

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
            ContentChild2.Children.Add(_rptRealisasiView);
        }
    }
}
