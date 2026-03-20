using PhishingApp.Commands;
using PhishingApp.Database;
using PhishingApp.Model;
using PhishingApp.Service;
using System.Windows.Input;

namespace PhishingApp.ViewModel
{
    public class EmailSendingViewModel
    {
        private readonly PhishingAppDbContext _dbContext;

        public EmailModel EmailModel { get; set; }

        public ValidationModel ValidationModelChangeLinks { get; set; }
        public ValidationModel ValidationModelAddLink { get; set; }
        public ValidationModel ValidationModelPreview { get; set; }
        public ValidationModel ValidationModelSendEmail { get; set; }

        public ICommand ImportEmailAddressesCommand { get; }
        public ICommand SendEmailCommand { get; }
        public ICommand ImportEmailTemplateCommand { get; }
        public ICommand PreviewEmailCommand { get; }
        public ICommand ChangeLinksToMaliciousCommand { get; }
        public ICommand AddImageCommand { get; }
        public ICommand StatisticCommand { get; }
        public ICommand ShowVictimsCommand { get; }
        public ICommand AddLinkCommand { get; }

        public EmailSendingViewModel()
        {
            _dbContext = new PhishingAppDbContext();

            WebsiteService websiteService = new WebsiteService(_dbContext);
            VictimService victimService = new VictimService(_dbContext);

            EmailModel = new EmailModel();

            ValidationModelChangeLinks = new ValidationModel();
            ValidationModelAddLink = new ValidationModel();
            ValidationModelPreview = new ValidationModel();
            ValidationModelSendEmail = new ValidationModel();

            SendEmailCommand = new SendEmailCommand(EmailModel, ValidationModelSendEmail, websiteService);
            ImportEmailTemplateCommand = new ImportEmailTemplateCommand(EmailModel);
            PreviewEmailCommand = new PreviewEmailCommand(EmailModel, ValidationModelPreview);
            ChangeLinksToMaliciousCommand = new ChangeLinksToMaliciousCommand(EmailModel, ValidationModelChangeLinks);
            ImportEmailAddressesCommand = new ImportEmailAddressesCommand(EmailModel);
            AddImageCommand = new AddImageCommand(EmailModel);
            AddLinkCommand = new AddLinkCommand(EmailModel, ValidationModelAddLink);
        }
    }
}


