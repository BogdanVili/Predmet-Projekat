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

        private int sentMails;

        public int SentMails
        {
            get { return sentMails; }
            set
            {
                sentMails = value;
                OnPropertyChanged("SentMails");
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

        private Dictionary<string, Victim> victims;

        public Dictionary<string, Victim> Victims
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
            sentMails = 0;
            formsFilled = 0;
            Victims = new Dictionary<string, Victim>();

            _dispatcher = Dispatcher.CurrentDispatcher;
            FormDataService.OnFormFilled += OnFormFilledReceived;
        }

        private void OnFormFilledReceived(Victim victim)
        {
            //Coming from a WCF thread so OnPropertyChanged isnt being recognized
            _dispatcher.Invoke(() =>
            {
                Victims.Add(victim.Email, victim);
                FormsFilled++;
            });
        }
    }
}
