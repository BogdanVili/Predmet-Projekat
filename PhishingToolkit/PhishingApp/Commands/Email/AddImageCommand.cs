using Microsoft.Win32;
using MimeKit.Utils;
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

        public string BrowseImageFiles()
        {
            OpenFileDialog dlg = new OpenFileDialog() { Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp" };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }

            return "";
        }

        public void AddImageToBody(string path)
        {
            if (path == "")
                return;


            _emailModel.BodyBuilder.TextBody = _emailModel.Body;

            if (_emailModel.Body != null)
            {
                if (_emailModel.HtmlBody == null)
                {
                    _emailModel.HtmlBody = "\n" + "<p  style=\"white-space: pre-line\">" + _emailModel.Body + "</p>" + "\n";
                    _emailModel.HtmlBodyHelper = _emailModel.Body;
                    // za preview
                    _emailModel.HtmlForPreview = "\n" + "<p  style=\"white-space: pre-line\">" + _emailModel.Body + "</p>" + "\n";
                }
                else
                {
                    string temp = _emailModel.Body.Substring(_emailModel.HtmlBodyHelper.Length);

                    if (temp == "")
                    {

                    }
                    else
                    {
                        _emailModel.HtmlBody += "\n" + "<p  style=\"white-space: pre-line\">" + temp + " </p>" + "\n";
                        // za preview
                        _emailModel.HtmlForPreview += "\n" + "<p  style=\"white-space: pre-line\">" + temp + " </p>" + "\n";
                        _emailModel.HtmlBodyHelper = _emailModel.Body;
                    }
                }

            }



            var image = _emailModel.BodyBuilder.LinkedResources.Add(path);
            image.ContentId = MimeUtils.GenerateMessageId();

            _emailModel.HtmlBody += string.Format(@"{2} <left><img src=""cid:{1}""></left> {2}", _emailModel.Body, image.ContentId, Environment.NewLine);
            _emailModel.HtmlBodyHelper = _emailModel.HtmlBody;

            // za preview
            _emailModel.HtmlForPreview += string.Format(@"{1} <left><img src=""{0}""></left> {1}", path, Environment.NewLine);

            _emailModel.BodyBuilder.HtmlBody = _emailModel.HtmlBody;

            _emailModel.BodyBuilder.Attachments.Add(path);

            _emailModel.Body = _emailModel.BodyBuilder.HtmlBody;


        }

    }
}
