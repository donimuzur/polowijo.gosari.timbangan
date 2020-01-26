using CrystalDecisions.CrystalReports.Engine;
using polowijo.gosari.timbangan.core;
using polowijo.gosari.timbangan.Model;
using polowijo.gosari.timbangan.UI.HOME;
using polowijo.gosari.timbangan.UI.LOGIN;
using polowijo.gosari.timbangan.UI.SETTING;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace polowijo.gosari.timbangan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        private Login_Window _loginWindow;
        private Home_Menu _homeMenu;
        private Setting_View _settingView;
        public MainWindow()
        {
            CustomMapper Mapper = new CustomMapper();
            _loginWindow = new Login_Window();
            _homeMenu = new Home_Menu();
            _settingView = new Setting_View();
            InitializeComponent();
            Init();
            CheckSession();
        }
        private void CheckSession()
        {
            BaseController.CurrentUser = null;
            this.Hide();
            _loginWindow = new Login_Window();
            if (_loginWindow.ShowDialog() == true)
            {
                this.Show();
            }
            else
            {
                Close();
                System.Environment.Exit(0);
            }
        }
        private void Init()
        {
            if (CheckConnection())
            {
                MainView.Children.Clear();
                MainView.Children.Add(_homeMenu);
            }
        }
        private bool CheckConnection()
        {
            try
            {
                var connectString = ConfigurationManager.ConnectionStrings["TIMBANGANEntities"].ConnectionString;
                var entityStringBuilder = new EntityConnectionStringBuilder(connectString);
                SqlConnectionStringBuilder SqlConnection = new SqlConnectionStringBuilder(entityStringBuilder.ProviderConnectionString);

                using (var connection = new SqlConnection(SqlConnection.ConnectionString))
                {
                    var query = "select 1";
                    Console.WriteLine("Executing: {0}", query);

                    var command = new SqlCommand(query, connection);
                    connection.Open();
                    command.ExecuteScalar();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogError.WriteError(ex);
                return false;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure ?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
                Environment.Exit(0);
            }
            else
            {

            }
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void Lv_Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var IdxSelected = Lv_Menu.SelectedIndex;
            if (IdxSelected == 0)
            {
                if (MainView != null)
                {
                    MainView.Children.Clear();
                    if (CheckConnection())
                    {
                        _homeMenu = new Home_Menu();
                        MainView.Children.Add(_homeMenu);

                        CaptionHeader.Text = "Menu";
                    }
                }
            }
            else if (IdxSelected == 1)
            {
                //MainView.Children.Clear();
                //if (CheckConnection())
                //{
                //    _masterMenu = new MASTER_MENU();
                //    MainView.Children.Add(_masterMenu);
                //}
            }
            else if (IdxSelected == 2)
            {
                MainView.Children.Clear();
                _settingView = new Setting_View();
                MainView.Children.Add(_settingView);

            }
            else if (IdxSelected == 3)
            {
               
            }
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            CheckSession();
        }
    }
}
