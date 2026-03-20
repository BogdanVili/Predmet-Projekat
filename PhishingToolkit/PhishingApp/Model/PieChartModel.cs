using LiveCharts;
using System;
using System.ComponentModel;

namespace PhishingApp.Model
{
    public class PieChartModel : INotifyPropertyChanged
    {
        private readonly StatisticsModel _statisticsModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChartValues<int> Values { get; set; }
        public string[] Labels { get; set; }

        public Func<ChartPoint, string> PointLabel { get; set; }

        public PieChartModel(StatisticsModel statisticsModel)
        {
            _statisticsModel = statisticsModel;

            Values = new ChartValues<int>
        {
            _statisticsModel.EmailsSent,
            _statisticsModel.FormsFilled
        };

            Labels = new[]
            {
            "Mails sent",
            "Forms filled"
        };

            PointLabel = chartPoint => chartPoint.X.ToString();

            _statisticsModel.PropertyChanged += OnStatisticsChanged;
        }

        private void OnStatisticsChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(StatisticsModel.EmailsSent))
                Values[0] = _statisticsModel.EmailsSent;

            if (args.PropertyName == nameof(StatisticsModel.FormsFilled))
                Values[1] = _statisticsModel.FormsFilled;
        }
    }
}
