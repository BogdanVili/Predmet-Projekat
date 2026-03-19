using PhishingApp.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class ChangeLinksCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private readonly EmailModel _emailModel;

        private readonly ValidationModel _validationModel;

        public ChangeLinksCommand(EmailModel emailModel, ValidationModel validationModel)
        {
            _emailModel = emailModel;
            _validationModel = validationModel;
        }


        public bool CanExecute(object parameter)
        {
            _validationModel.Text = string.Empty;
            string temp = "";
            if (_emailModel.MaliciousLink == string.Empty)
            {
                temp += "Malicious link is empty" + "\n";
            }
            if (_emailModel.Body == null)
            {
                temp += "Email body is empty";
            }

            _validationModel.Text = temp;

            if (_emailModel.MaliciousLink == string.Empty || _emailModel.Body == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!_emailModel.MaliciousLink.Contains("http://") && !_emailModel.MaliciousLink.Contains("https://"))
            {
                _emailModel.MaliciousLink = "http://" + _emailModel.MaliciousLink;
            }

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(_emailModel.Body);
            foreach (var node in htmlDoc.DocumentNode.SelectNodes("//a"))
            {
                node.SetAttributeValue("href", _emailModel.MaliciousLink);
            }
            var changedHtml = htmlDoc.DocumentNode.WriteTo();
            _emailModel.Body = changedHtml;

            MessageBox.Show("Changed links to: " + _emailModel.MaliciousLink);
        }

    }
}
