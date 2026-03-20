using Microsoft.Win32;
using PhishingApp.Helpers;
using PhishingApp.Model;
using System;
using System.IO;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class ImportEmailAddressesCommand : ICommand
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

        public ImportEmailAddressesCommand(EmailModel emailModel)
        {
            _emailModel = emailModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string path = BrowseTxtFiles();

            if (path == "")
            {
                return;
            }

            LoadEmails(path);
        }

        private string BrowseTxtFiles()
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                DefaultExt = ".txt",
                Filter = "Text|*.txt|All|*.*",
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }

            return "";
        }

        private void LoadEmails(string path)
        {
            if (path == "")
                return;

            string emails = _emailModel.Emails;

            try
            {
                string email = string.Empty;
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((email = sr.ReadLine()) != null)
                    {
                        try
                        {
                            if (EmailHelper.IsValidEmail(email))
                            {
                                emails += email + "\n";
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }


            _emailModel.Emails = emails;
        }
    }
}
