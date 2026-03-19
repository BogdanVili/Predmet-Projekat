using LiveCharts;
using PhishingApp.Commands;
using PhishingApp.Database;
using PhishingApp.Model;
using PhishingApp.Service;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace PhishingApp.ViewModel
{
    public class MainViewModel
    {
        private readonly PhishingAppDbContext _dbContext;
        private readonly DispatcherTimer _timer;

        public EmailModel EmailModel { get; set; }
        public StatisticsModel StatisticsModel { get; set; }

        public ValidationModel ValidationModelChangeLinks { get; set; }
        public ValidationModel ValidationModelAddLink { get; set; }
        public ValidationModel ValidationModelPreview { get; set; }
        public ValidationModel ValidationModelSendEmail { get; set; }

        public PieChartModel PieChartModel { get; set; }

        public Func<ChartPoint, string> PointLabel { get; set; }

        public ICommand EmailReadCommand { get; }
        public ICommand SendEmailCommand { get; }
        public ICommand ParseEmailCommand { get; }
        public ICommand PreviewEmailCommand { get; }
        public ICommand ChangeLinksCommand { get; }
        public ICommand AddImageCommand { get; }
        public ICommand StatisticCommand { get; }
        public ICommand InitializeStatisticsCommand { get; }
        public ICommand ShowVictimsCommand { get; }
        public ICommand AddLinkCommand { get; }

        public MainViewModel()
        {
            _dbContext = new PhishingAppDbContext();

            WebsiteService websiteService = new WebsiteService(_dbContext);
            VictimService victimService = new VictimService(_dbContext);

            EmailModel = new EmailModel();
            StatisticsModel = new StatisticsModel();

            ValidationModelChangeLinks = new ValidationModel();
            ValidationModelAddLink = new ValidationModel();
            ValidationModelPreview = new ValidationModel();
            ValidationModelSendEmail = new ValidationModel();

            PieChartModel = new PieChartModel(StatisticsModel);

            PointLabel = chartPoint =>
                $"{chartPoint.Y} ({chartPoint.Participation:P})";

            InitializeStatisticsCommand = new InitializeStatisticsCommand(StatisticsModel, websiteService, victimService);
            ShowVictimsCommand = new ShowVictimsCommand(StatisticsModel);

            SendEmailCommand = new SendEmailCommand(EmailModel, StatisticsModel, PieChartModel, ValidationModelSendEmail, websiteService);
            ParseEmailCommand = new ImportEmailTemplateCommand(EmailModel);
            PreviewEmailCommand = new PreviewEmailCommand(EmailModel, ValidationModelPreview);
            ChangeLinksCommand = new ChangeLinksCommand(EmailModel, ValidationModelChangeLinks);
            EmailReadCommand = new ImportEmailAddressesCommand(EmailModel);
            AddImageCommand = new AddImageCommand(EmailModel);
            AddLinkCommand = new AddLinkCommand(EmailModel, ValidationModelAddLink);

            InitializeStatisticsCommand.Execute(null);
        }
    }
}


