using LiveCharts;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using PhishingApp.Model;
using PhishingApp.Service;
using System;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PhishingApp.Commands
{
    public class SendEmailCommand : ICommand
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

        private readonly StatisticsModel _statisticsModel;

        private readonly PieChartModel _pieChartModel;

        private readonly ValidationModel _validationModelSendEmail;

        private readonly WebsiteService _websiteService;

        public SendEmailCommand(EmailModel em, StatisticsModel sm, PieChartModel pcm, ValidationModel vm, WebsiteService websiteService)
        {
            _emailModel = em;
            _statisticsModel = sm;
            _pieChartModel = pcm;
            _validationModelSendEmail = vm;
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
            if (_emailModel.SenderEmail == string.Empty)
            {
                temp += "Email of sender is empty" + "\n";
            }
            if (_emailModel.SenderPassword == string.Empty)
            {
                temp += "Password of sender is empty" + "\n";
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


            if (_emailModel.Emails == null || _emailModel.SenderEmail.Equals("") || _emailModel.SenderName.Equals("") ||
                _emailModel.SenderPassword.Equals("") || _emailModel.RecipientName.Equals("") || _emailModel.Body == null ||
                _emailModel.EmailSubject.Equals(""))
            {
                return false;
            }

            return true;
        }

        public async void Execute(object parameter)
        {
            string[] emailArray;
            emailArray = _emailModel.Emails.Split('\n');
            //Last string of array ends up being /n
            if (emailArray[emailArray.Length - 1] == "\n")
                Array.Resize(ref emailArray, emailArray.Length - 1);


            //when hitting enter in textbox \r is put
            for (int i = 0; i < emailArray.Length; i++)
            {
                emailArray[i] = emailArray[i].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            }

            string smtpHost = ConfigurationManager.AppSettings.Get("smtpHost");
            int smtpPort = Int32.Parse(ConfigurationManager.AppSettings.Get("smtpPort"));
            bool smtpUseSSL = Boolean.Parse(ConfigurationManager.AppSettings.Get("smtpUseSSL"));



            foreach (string email in emailArray)
            {
                if (!email.Equals("") || !IsValidEmail(email))
                {
                    MessageBox.Show("Not all mails in the list are in a valid email format!");
                    return;
                }
            }


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

                    if (temp == "")
                    {

                    }
                    else
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



            foreach (string email in emailArray)
            {
                _emailModel.MessageToSend.From.Add(new MailboxAddress(_emailModel.SenderName, _emailModel.SenderEmail));
                _emailModel.MessageToSend.To.Add(new MailboxAddress(_emailModel.RecipientName, email));
                _emailModel.MessageToSend.Subject = _emailModel.EmailSubject;


                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(smtpHost, smtpPort, smtpUseSSL); // mozda 465
                    // Note: only needed if the SMTP server requires authentication
                    try
                    {
                        client.Authenticate(_emailModel.SenderEmail, _emailModel.SenderPassword);

                    }
                    catch (AuthenticationException)
                    {
                        _emailModel.Validate = "Invalid email or password, try again";
                        MessageBox.Show("Invalid email or password, try again");
                        return;
                    }

                    try
                    {
                        await client.SendAsync(_emailModel.MessageToSend);

                    }
                    catch (SmtpCommandException) { }

                    client.Disconnect(true);
                }

            }

            var currentSentMails = emailArray.Length;

            await _websiteService.IncrementEmailsSent(1, currentSentMails);
            var website = await _websiteService.GetWebsiteById(1);

            _statisticsModel.SentMails = website.EmailsSent;

            _pieChartModel.SentMailsSeries = new ChartValues<int>() { emailArray.Length };

            MessageBox.Show("Messages sent.");
        }


        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                Console.WriteLine(e);
                return false;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
