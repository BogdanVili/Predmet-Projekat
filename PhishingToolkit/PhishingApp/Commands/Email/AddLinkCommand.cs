using PhishingApp.Helpers;
using PhishingApp.Model;
using System;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class AddLinkCommand : ICommand
    {
        private readonly EmailModel _emailModel;

        private readonly ValidationModel _validationModel;

        public AddLinkCommand(EmailModel emailModel, ValidationModel validationModel)
        {
            _emailModel = emailModel;
            _validationModel = validationModel;
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
            _validationModel.Text = string.Empty;
            string temp = "";
            if (_emailModel.LinkToAdd == string.Empty)
            {
                temp += "Link is empty" + "\n";
            }
            if (_emailModel.TextForLink == string.Empty)
            {
                temp += "Text for link is empty";
            }

            _validationModel.Text = temp;

            if (_emailModel.LinkToAdd == "" || _emailModel.TextForLink == "")
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!_emailModel.LinkToAdd.Contains("http://") && !_emailModel.LinkToAdd.Contains("https://"))
            {
                _emailModel.LinkToAdd = "http://" + _emailModel.LinkToAdd;
            }

            _emailModel.BodyBuilder.TextBody = _emailModel.Body;

            if (_emailModel.Body != null)
            {
                EmailHelper.FormatBody(_emailModel);
            }

            _emailModel.HtmlBody += string.Format(@"<a href=""{0}""\>{1}</a>", _emailModel.LinkToAdd, _emailModel.TextForLink);

            // za preview
            _emailModel.HtmlForPreview += string.Format(@"<a href=""{0}"">{1}</a>", _emailModel.LinkToAdd, _emailModel.TextForLink);

            // zato sto prikazujes html u body mora ova linija koda
            _emailModel.HtmlBodyHelper = _emailModel.HtmlBody;


            _emailModel.BodyBuilder.HtmlBody = _emailModel.HtmlBody;
            _emailModel.Body = _emailModel.BodyBuilder.HtmlBody;
        }
    }
}
