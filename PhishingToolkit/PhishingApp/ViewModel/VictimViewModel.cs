using LiveCharts;
using PhishingApp.Commands;
using PhishingApp.Database;
using PhishingApp.Model;
using PhishingApp.Service;
using System;
using System.Windows.Input;

namespace PhishingApp.ViewModel
{
    public class VictimViewModel
    {
        private readonly PhishingAppDbContext _dbContext;

        public StatisticsModel StatisticsModel { get; set; }
        public PieChartModel PieChartModel { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        public ICommand InitializeStatisticsCommand { get; }

        public VictimViewModel()
        {
            StatisticsModel = new StatisticsModel();

            PieChartModel = new PieChartModel(StatisticsModel);
            PointLabel = chartPoint =>
                $"{chartPoint.Y} ({chartPoint.Participation:P})";

            _dbContext = new PhishingAppDbContext();

            WebsiteService websiteService = new WebsiteService(_dbContext);
            VictimService victimService = new VictimService(_dbContext);


            InitializeStatisticsCommand = new InitializeStatisticsCommand(StatisticsModel, websiteService, victimService);

            InitializeStatisticsCommand.Execute(null);
        }
    }
}
