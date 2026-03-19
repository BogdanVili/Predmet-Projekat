using Common;
using PhishingApp.Config;
using PhishingApp.Helpers;
using PhishingApp.Service;
using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;

namespace PhishingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            StartWcfService();

            StartStaticService();

            IpCountryLookupService.Instance.Load(Path.Combine("..", "..", "Helpers", "ip_country_ranges.csv"));
        }

        private void StartWcfService()
        {
            var wcfServiceUri = new Uri(ConfigurationManager.AppSettings.Get("wcfServiceUri"));
            ServiceHost wcfService = new ServiceHost(
                typeof(FormDataService),
                wcfServiceUri
            );

            var wcfEndpoint = wcfService.AddServiceEndpoint(
                typeof(IFormMessage),
                new WebHttpBinding(),
                "api"
            );

            wcfEndpoint.EndpointBehaviors.Add(new WebHttpBehavior());
            wcfEndpoint.EndpointBehaviors.Add(new CorsBehavior());

            wcfService.Open();
        }

        private void StartStaticService()
        {
            var staticServiceUri = ConfigurationManager.AppSettings.Get("staticServiceUri");
            string webpagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "webpages");
            var staticService = new StaticFileService(webpagesFolder, staticServiceUri);
            staticService.Start();
        }
    }
}
