using OpenRiaServices.Client;
using OpenRiaServices.Client.Authentication;
using SimpleApplication.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfCore.Client;

namespace WpfCore
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            DomainContext.DomainClientFactory = new OpenRiaServices.Client.Web.SoapDomainClientFactory()
            {
                ServerBaseUri = new Uri(Uri.Text, UriKind.Absolute)
            };

            // Create a WebContext and add it to the ApplicationLifetimeObjects collection.
            // This will then be available as WebContext.Current.
            WebContext.Current.Authentication = new FormsAuthentication()
            {
                DomainContext = new SimpleApplication.Web.AspNetIdentityAuthenticationDomainContext()
            };

            LoginButton.IsEnabled = false;
            WebContext.Current.Authentication.Login(new LoginParameters(
                this.UserName.Text, this.Password.Text), OnLoginCompleted, null);
        }

        private void OnLoginCompleted(LoginOperation obj)
        {
            LoginButton.IsEnabled = true;
            if (obj.HasError)
            {
                MessageBox.Show($"Login failed: {obj.Error.Message}");
                obj.MarkErrorAsHandled();
            }
            else
            {
                if (obj.LoginSuccess)
                {
                    this.NavigationService.Navigate(new MainPage());
                }
                else
                {
                    MessageBox.Show($"LoginSuccess: false");
                }
            }
        }
    }
}
