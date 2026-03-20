using MimeKit;
using PhishingApp.Model;
using System;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class ImportEmailTemplateCommand : ICommand
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

        public ImportEmailTemplateCommand(EmailModel emailModel)
        {
            _emailModel = emailModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string path = BrowseEMLFiles();

            if (path == "")
            {
                return;
            }

            ParseEMLFile(path);
        }

        private string BrowseEMLFiles()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".eml",
                Filter = "Eml|*.eml|All|*.*"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }

            return "";
        }

        private void ParseEMLFile(string path)
        {
            if (path == "")
                return;

            var message = MimeMessage.Load(path);

            _emailModel.Body = message.HtmlBody;

            _emailModel.HtmlImported = true;
        }
    }
}
