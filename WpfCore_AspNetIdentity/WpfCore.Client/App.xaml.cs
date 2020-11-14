using OpenRiaServices.Client;
using OpenRiaServices.Client.Authentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfCore.Client;

namespace WpfCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            DomainContext.DomainClientFactory = new OpenRiaServices.Client.Web.SoapDomainClientFactory()
            {
                ServerBaseUri = new Uri("http://localhost:57294/Services/", UriKind.Absolute)
            };

            // Create a WebContext and add it to the ApplicationLifetimeObjects collection.
            // This will then be available as WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication()
            {
                 DomainContext = new SimpleApplication.Web.AspNetIdentityAuthenticationDomainContext()
            };
        }
    }
}
