using PhishingApp.Commands;
using PhishingApp.Service;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;

namespace PhishingApp.Model
{
    public class StatisticsModel : INotifyPropertyChanged
    {
        private readonly Dispatcher _dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int emailsSent;

        public int EmailsSent
        {
            get { return emailsSent; }
            set
            {
                emailsSent = value;
                OnPropertyChanged("EmailsSent");
            }
        }

        private int formsFilled;

        public int FormsFilled
        {
            get { return formsFilled; }
            set
            {
                formsFilled = value;
                OnPropertyChanged("FormsFilled");
            }
        }

        private List<Victim> victims;

        public List<Victim> Victims
        {
            get { return victims; }
            set
            {
                victims = value;
                OnPropertyChanged("Victims");
            }
        }

        public StatisticsModel()
        {
            emailsSent = 0;
            formsFilled = 0;
            Victims = new List<Victim>();

            _dispatcher = Dispatcher.CurrentDispatcher;
            FormDataService.OnFormFilled += OnFormFilledReceived;
            SendEmailCommand.OnEmailsSent += OnEmailsSentReceived;
        }

        private void OnFormFilledReceived(Victim victim)
        {
            //Coming from a WCF thread so OnPropertyChanged isnt being recognized
            _dispatcher.Invoke(() =>
            {
                Victims.Add(victim);
                FormsFilled++;
            });
        }

        private void OnEmailsSentReceived(int emailsSentCount)
        {
            EmailsSent++;
        }
    }
}
