using polowijo.gosari.timbangan.core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace polowijo.gosari.timbangan.UI.SETTING
{
    /// <summary>
    /// Interaction logic for Setting_View.xaml
    /// </summary>
    public partial class Setting_View : UserControl
    {
        string Server = "";
        string UserId = "";
        string Password = "";
        string metadata = "";
        string Database = "";
        public Setting_View()
        {
            InitializeComponent();
        }

        private void On_Load(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            var connectString = ConfigurationManager.ConnectionStrings["TIMBANGANEntities"].ConnectionString;
            var entityStringBuilder = new EntityConnectionStringBuilder(connectString);
            SqlConnectionStringBuilder SqlConnection = new SqlConnectionStringBuilder(entityStringBuilder.ProviderConnectionString);

            cfg_Server.Text = SqlConnection.DataSource;
            cfg_UserId.Text = SqlConnection.UserID;
            cfg_Database.Text = SqlConnection.InitialCatalog;
            cfg_Password.Password = "";

            Server = cfg_Server.Text;
            UserId = cfg_UserId.Text;
            Password = cfg_Password.Password;
            Database = cfg_Database.Text;
            metadata = entityStringBuilder.Metadata;
            Mouse.OverrideCursor = null;
        }
       
        private void Button_Test_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (CheckConnection())
            {
                MessageBox.Show("Tes Koneksi Sukses", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                Mouse.OverrideCursor = null;
                return;
            }
            MessageBox.Show("Tes Koneksi Gagal", "Gagal", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            Mouse.OverrideCursor = null ;
            return;
        }
        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                var connectString = ConfigurationManager.ConnectionStrings["TIMBANGANEntities"].ConnectionString;
                var entityStringBuilder = new EntityConnectionStringBuilder(connectString);
                SqlConnectionStringBuilder SqlConnection = new SqlConnectionStringBuilder(entityStringBuilder.ProviderConnectionString);

                SqlConnection.DataSource = cfg_Server.Text;
                SqlConnection.UserID = cfg_UserId.Text;
                SqlConnection.Password = cfg_Password.Password;
                SqlConnection.InitialCatalog = cfg_Database.Text;

                entityStringBuilder.ProviderConnectionString = SqlConnection.ConnectionString;

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings["TIMBANGANEntities"].ConnectionString = entityStringBuilder.ConnectionString;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");

                MessageBox.Show("Setting berhasil disimpan.\n aplikasi akan di restart", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                Process.Start(Application.ResourceAssembly.Location);
                Mouse.OverrideCursor = null;
                System.Windows.Application.Current.Shutdown();
                return;

            }
            catch (Exception exp)
            {
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessage.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.WriteError(exp);
                return;
            }
        }

        private bool CheckConnection()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
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
                Mouse.OverrideCursor = null;
                return true;
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = null;
                LogError.WriteError(ex);
                return false;
            }
        }
    }
}
