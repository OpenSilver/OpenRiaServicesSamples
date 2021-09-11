using CustomEndpoint.Web;
using OpenRiaServices.DomainServices.Client;
using OpenRiaServices.DomainServices.Client.ApplicationServices;
using OpenRiaServices.DomainServices.Client.Web;
using System;
using System.Windows;

namespace CustomEndpoint.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;

#if OPENSILVER
            this.InitializeComponent();

            // Enter construction logic here...

            DomainContext.DomainClientFactory = new OpenRiaServices.DomainServices.Client.Web.SoapDomainClientFactory()
            {
                ServerBaseUri = new Uri("http://localhost:51359/"),
            };

            var mainPage = new MainPage();
            Window.Current.Content = mainPage;
#endif
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            // DomainClientFactory and DomainContext for authentication is setup at login time
            // Create a WebContext and add it to the ApplicationLifetimeObjects collection.
            // This will then be available as WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication()
            {
                DomainContextType = typeof(AuthenticationDomainService1).AssemblyQualifiedName
            };
        }
    }
}