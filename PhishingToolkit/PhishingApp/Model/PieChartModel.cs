using LiveCharts;
using System.ComponentModel;

namespace PhishingApp.Model
{
    public class PieChartModel : INotifyPropertyChanged
    {
        private readonly StatisticsModel _statisticsModel;

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        private ChartValues<int> sentMailsSeries;

        public ChartValues<int> SentMailsSeries
        {
            get { return sentMailsSeries; }
            set
            {
                sentMailsSeries = value;
                OnPropertyChanged("SentMailsSeries");
            }

        }

        private ChartValues<int> formsFilledSeries;

        public ChartValues<int> FormsFilledSeries
        {
            get { return formsFilledSeries; }
            set
            {
                formsFilledSeries = value;
                OnPropertyChanged("FormsFilledSeries");
            }
        }



        public PieChartModel(StatisticsModel statisticsModel)
        {
            _statisticsModel = statisticsModel;

            FormsFilledSeries = new ChartValues<int>() { statisticsModel.FormsFilled };
            SentMailsSeries = new ChartValues<int>() { statisticsModel.SentMails };

            _statisticsModel.PropertyChanged += OnStatisticsChanged;
        }

        private void OnStatisticsChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(StatisticsModel.FormsFilled))
                FormsFilledSeries[0] = _statisticsModel.FormsFilled;

            if (args.PropertyName == nameof(StatisticsModel.SentMails))
                SentMailsSeries[0] = _statisticsModel.SentMails;
        }
    }
}
