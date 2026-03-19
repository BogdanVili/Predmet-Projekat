using PhishingApp.Model;
using PhishingApp.Views;
using System;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class ShowVictimsCommand : ICommand
    {

        private readonly StatisticsModel _statisticsModel;

        public ShowVictimsCommand(StatisticsModel statisticsModel)
        {
            _statisticsModel = statisticsModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VictimsWindow victimsWindow = new VictimsWindow(_statisticsModel);
            victimsWindow.Show();
        }
    }
}
