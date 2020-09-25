using AutoMapper;
using polowijo.gosari.timbangan.core;
using polowijo.gosari.timbangan.dal;
using polowijo.gosari.timbangan.dal.IServices;
using polowijo.gosari.timbangan.dal.Services;
using polowijo.gosari.timbangan.dto;
using polowijo.gosari.timbangan.Model;
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

namespace polowijo.gosari.timbangan.UI.LOGIN
{
    /// <summary>
    /// Interaction logic for Login_Window.xaml
    /// </summary>
    public partial class Login_Window : Window
    {
        private ILoginServices _loginServices;
        
        public Login_Window()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            _loginServices = new LoginServices();
            InitializeComponent();
            txtUsername.Focus();
            Mouse.OverrideCursor =null;
        }
        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSubmit_Click(sender, e);
            }
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            try
            {
                var Dto = Mapper.Map<LoginDto>(new LoginModel { USERNAME = txtUsername.Text, USER_ID=txtUsername.Text, PASSWORD=txtPassword.Password });
                var GetData = _loginServices.VerificationLogin(Dto);
                if (GetData != null)
                {
                    BaseController.CurrentUser = new Login();
                    BaseController.CurrentUser.FIRST_NAME = GetData.FIRST_NAME;
                    BaseController.CurrentUser.LAST_NAME = GetData.LAST_NAME;
                    BaseController.CurrentUser.PASSWORD = GetData.PASSWORD;
                    BaseController.CurrentUser.STATUS = GetData.STATUS;
                    BaseController.CurrentUser.USERNAME = GetData.USERNAME;
                    BaseController.CurrentUser.USER_ID = GetData.USER_ID;
                    BaseController.CurrentUser.EMAIL = GetData.EMAIL;
                    BaseController.CurrentUser.LAST_ONLINE = GetData.LAST_ONLINE;
                    BaseController.CurrentUser.POSITION = GetData.POSITION;
                    if(GetData.ROLE_ID != null)
                    {
                        BaseController.CurrentUser.ROLE_ID = (int)GetData.ROLE_ID;
                    }                   

                    _loginServices.SetLastOnline(BaseController.CurrentUser.USER_ID);

                    DialogResult = true;
                    Close();
                }
                lblError.Content = "Username atau password salah";
                Mouse.OverrideCursor = null;
            }
            catch (Exception exp)
            {
                MessageBox.Show(Constans.SubmitMessage.Error, Constans.SubmitMessage.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.WriteError(exp);
                Mouse.OverrideCursor = null;
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            Environment.Exit(0);
        }
    }
}
