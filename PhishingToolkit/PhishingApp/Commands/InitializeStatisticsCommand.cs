using PhishingApp.Model;
using PhishingApp.Service;
using System;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class InitializeStatisticsCommand : ICommand
    {
        private readonly WebsiteService _websiteService;
        private readonly VictimService _victimService;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private readonly StatisticsModel _statisticsModel;

        public InitializeStatisticsCommand(StatisticsModel statisticsModel, WebsiteService websiteService, VictimService victimService)
        {
            _statisticsModel = statisticsModel;
            _websiteService = websiteService;
            _victimService = victimService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var victims = await _victimService.GetAllVictims();
            foreach (var victim in victims)
            {
                _statisticsModel.Victims[victim.Email] = victim;
            }

            var websiteData = await _websiteService.GetWebsiteById(1);
            _statisticsModel.FormsFilled = websiteData.FormsFilled;
            _statisticsModel.SentMails = websiteData.EmailsSent;
        }
    }
}
