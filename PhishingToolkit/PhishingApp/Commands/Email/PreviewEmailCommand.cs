using PhishingApp.Helpers;
using PhishingApp.Model;
using System;
using System.IO;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class PreviewEmailCommand : ICommand
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

        public PreviewEmailCommand(EmailModel emailModel, ValidationModel validationModel)
        {
            _emailModel = emailModel;
            _validationModel = validationModel;
        }

        public bool CanExecute(object parameter)
        {
            _validationModel.Text = string.Empty;
            string temp = "";
            if (_emailModel.Body == null)
            {
                temp += "Email body is empty";
            }

            _validationModel.Text = temp;


            if (_emailModel.Body == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (_emailModel.HtmlImported)
            {
                using (StreamWriter sw = new StreamWriter("PreviewHtml.html"))
                {
                    sw.Write(_emailModel.Body);
                }

                System.Diagnostics.Process.Start("PreviewHtml.html");
            }
            else
            {
                _emailModel.BodyBuilder.TextBody = _emailModel.Body;

                if (_emailModel.Body != null)
                {
                    EmailHelper.FormatBody(_emailModel);
                }

                // zato sto prikazujes html u body mora ova linija koda
                _emailModel.HtmlBodyHelper = _emailModel.HtmlBody;


                _emailModel.BodyBuilder.HtmlBody = _emailModel.HtmlBody;
                _emailModel.Body = _emailModel.BodyBuilder.HtmlBody;

                using (StreamWriter sw = new StreamWriter("Preview.html"))
                {
                    sw.Write(_emailModel.HtmlForPreview);
                }

                System.Diagnostics.Process.Start("Preview.html");
            }
        }
    }
}
