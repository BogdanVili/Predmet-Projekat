using Microsoft.Win32;
using MimeKit.Utils;
using PhishingApp.Helpers;
using PhishingApp.Model;
using System;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class AddImageCommand : ICommand
    {
        private readonly EmailModel _emailModel;

        public AddImageCommand(EmailModel emailModel)
        {
            _emailModel = emailModel;
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
            string image = BrowseImageFiles();

            if (image == "")
            {
                return;
            }

            AddImageToBody(image);
        }

        private string BrowseImageFiles()
        {
            OpenFileDialog dlg = new OpenFileDialog() { Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp" };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }

            return "";
        }

        private void AddImageToBody(string path)
        {
            if (path == "")
                return;


            _emailModel.BodyBuilder.TextBody = _emailModel.Body;

            if (_emailModel.Body != null)
            {
                EmailHelper.FormatBody(_emailModel);
            }

            var image = _emailModel.BodyBuilder.LinkedResources.Add(path);
            image.ContentId = MimeUtils.GenerateMessageId();

            _emailModel.HtmlBody += string.Format(@"{2} <left><img src=""cid:{1}""></left> {2}", _emailModel.Body, image.ContentId, Environment.NewLine);
            _emailModel.HtmlBodyHelper = _emailModel.HtmlBody;

            _emailModel.HtmlForPreview += string.Format(@"{1} <left><img src=""{0}""></left> {1}", path, Environment.NewLine);

            _emailModel.BodyBuilder.HtmlBody = _emailModel.HtmlBody;

            _emailModel.BodyBuilder.Attachments.Add(path);

            _emailModel.Body = _emailModel.BodyBuilder.HtmlBody;
        }
    }
}
