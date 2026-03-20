using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using PhishingApp.Helpers;
using PhishingApp.Model;
using PhishingApp.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class SendEmailCommand : ICommand
    {
        public static event Action<int> OnEmailsSent;

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

        private readonly ValidationModel _validationModelSendEmail;

        private readonly WebsiteService _websiteService;

        public SendEmailCommand(EmailModel emailModel, ValidationModel validationModel, WebsiteService websiteService)
        {
            _emailModel = emailModel;
            _validationModelSendEmail = validationModel;
            _websiteService = websiteService;
        }

        public bool CanExecute(object parameter)
        {
            _validationModelSendEmail.Text = string.Empty;
            string temp = "";
            if (_emailModel.Emails == null)
            {
                temp += "There are no emails to send message to" + "\n";
            }
            if (_emailModel.Body == null)
            {
                temp += "Email body is empty" + "\n";
            }
            if (_emailModel.SenderName == string.Empty)
            {
                temp += "Name of sender is empty" + "\n";
            }
            if (_emailModel.RecipientName == string.Empty)
            {
                temp += "Recipient name is empty" + "\n";
            }
            if (_emailModel.EmailSubject == string.Empty)
            {
                temp += "Subject of email is empty";
            }

            _validationModelSendEmail.Text = temp;


            if (_emailModel.Emails == null || _emailModel.SenderName.Equals("") || _emailModel.RecipientName.Equals("") || _emailModel.Body == null ||
                _emailModel.EmailSubject.Equals(""))
            {
                return false;
            }

            return true;
        }

        public async void Execute(object parameter)
        {
            List<string> emailAddresses = EmailAddressArrayFormatAndValidation();
            if (emailAddresses == null)
            {
                return;
            }

            EmailTemplateFormatting();

            string smtpHost = ConfigurationManager.AppSettings.Get("smtpHost");
            int smtpPort = Int32.Parse(ConfigurationManager.AppSettings.Get("smtpPort"));
            bool smtpUseSSL = Boolean.Parse(ConfigurationManager.AppSettings.Get("smtpUseSSL"));
            var senderEmail = ConfigurationManager.AppSettings.Get("senderEmail");
            var senderPassword = ConfigurationManager.AppSettings.Get("senderPassword");

            _emailModel.MessageToSend.From.Add(new MailboxAddress(_emailModel.SenderName, senderEmail));

            foreach (string email in emailAddresses)
            {
                _emailModel.MessageToSend.To.Add(new MailboxAddress(_emailModel.RecipientName, email));

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(smtpHost, smtpPort, smtpUseSSL);
                    try
                    {
                        client.Authenticate(senderEmail, senderPassword);
                    }
                    catch (AuthenticationException error)
                    {
                        MessageBox.Show($"Problem with your sender {error.Message}");
                        return;
                    }

                    try
                    {
                        await client.SendAsync(_emailModel.MessageToSend);
                    }
                    catch (SmtpCommandException) { }

                    client.Disconnect(true);
                }

                _emailModel.MessageToSend.To.RemoveAt(0);
            }

            var currentSentMails = emailAddresses.Count;

            await _websiteService.IncrementEmailsSent(1, currentSentMails);
            var website = await _websiteService.GetWebsiteById(1);

            OnEmailsSent?.Invoke(emailAddresses.Count);

            MessageBox.Show("Messages sent.");
        }

        private List<string> EmailAddressArrayFormatAndValidation()
        {
            List<string> emails = _emailModel.Emails.Split('\n').ToList();

            string lastElement = emails[emails.Count - 1];
            if (lastElement == "\n" || lastElement == "\r\n" || lastElement == "\r" || lastElement == "")
            {
                emails.RemoveAt(emails.Count - 1);
            }

            for (int i = 0; i < emails.Count; i++)
            {
                emails[i] = emails[i].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }

            emails.RemoveAll(e => e == null || e == "");

            foreach (string email in emails)
            {
                if (email.Equals("") || !EmailHelper.IsValidEmail(email))
                {
                    MessageBox.Show("Not all mails in the list are in a valid email format!");
                    return null;
                }
            }

            return emails;
        }

        private void EmailTemplateFormatting()
        {

            if (_emailModel.HtmlImported)
            {
                _emailModel.MessageToSend.Body = new TextPart("html") { Text = _emailModel.Body };
            }
            else
            {
                _emailModel.BodyBuilder.TextBody = _emailModel.Body;


                if (_emailModel.HtmlBody == null)
                {
                    _emailModel.HtmlBody = "\n" + "<p>" + _emailModel.Body + "</p>" + "\n";
                    _emailModel.HtmlBodyHelper = _emailModel.Body;
                }
                else
                {
                    string temp = _emailModel.Body.Substring(_emailModel.HtmlBodyHelper.Length);

                    if (temp != "")
                    {
                        _emailModel.HtmlBody += "\n" + "<p>" + temp + " </p>" + "\n";
                        _emailModel.HtmlBodyHelper = _emailModel.Body;
                    }
                }

                // zato sto prikazujes html u body mora ova linija koda
                _emailModel.HtmlBodyHelper = _emailModel.HtmlBody;


                _emailModel.BodyBuilder.HtmlBody = _emailModel.HtmlBody;
                _emailModel.Body = _emailModel.BodyBuilder.HtmlBody;


                _emailModel.MessageToSend.Body = _emailModel.BodyBuilder.ToMessageBody();
            }

            _emailModel.MessageToSend.Subject = _emailModel.EmailSubject;
        }

        public static bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
