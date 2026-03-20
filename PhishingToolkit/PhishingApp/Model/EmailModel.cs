using MimeKit;
using System.ComponentModel;

namespace PhishingApp.Model
{
    public class EmailModel : INotifyPropertyChanged
    {
        private bool htmlImported;

        public bool HtmlImported
        {
            get { return htmlImported; }
            set
            {
                htmlImported = value;
                OnPropertyChanged("HtmlImported");
            }
        }


        private string validate;

        public string Validate
        {
            get { return validate; }
            set
            {
                validate = value;
                OnPropertyChanged("Validate");
            }
        }

        private MimeMessage messageToSend;

        public MimeMessage MessageToSend
        {
            get { return messageToSend; }
            set { messageToSend = value; }
        }



        private string emails;

        public string Emails
        {
            get { return emails; }
            set
            {
                emails = value;
                OnPropertyChanged("Emails");
            }
        }

        private string body;

        public string Body
        {
            get { return body; }
            set
            {

                body = value;
                OnPropertyChanged("Body");
            }
        }

        private string maliciousLink;

        public string MaliciousLink
        {
            get { return maliciousLink; }
            set
            {
                maliciousLink = value;
                OnPropertyChanged("MaliciousLink");
            }
        }

        private string htmlBody;

        public string HtmlBody
        {
            get { return htmlBody; }
            set
            {
                htmlBody = value;
                OnPropertyChanged("HtmlBody");
            }
        }

        private string htmlBodyHelper;

        public string HtmlBodyHelper
        {
            get { return htmlBodyHelper; }
            set
            {
                htmlBodyHelper = value;
                OnPropertyChanged("HtmlBodyHelper");
            }
        }

        private string senderName;

        public string SenderName
        {
            get { return senderName; }
            set
            {
                senderName = value;
                OnPropertyChanged("SenderName");
            }
        }


        private string recipientName;

        public string RecipientName
        {
            get { return recipientName; }
            set
            {
                recipientName = value;
                OnPropertyChanged("RecipientName");
            }
        }

        private string emailSubject;

        public string EmailSubject
        {
            get { return emailSubject; }
            set
            {
                emailSubject = value;
                OnPropertyChanged("EmailSubject");
            }
        }

        private BodyBuilder bodyBuilder;

        public BodyBuilder BodyBuilder
        {
            get { return bodyBuilder; }
            set
            {
                bodyBuilder = value;
                OnPropertyChanged("BodyBuilder");
            }
        }

        private string linkToAdd;

        public string LinkToAdd
        {
            get { return linkToAdd; }
            set
            {
                linkToAdd = value;
                OnPropertyChanged("LinkToAdd");
            }
        }

        private string textForLink;

        public string TextForLink
        {
            get { return textForLink; }
            set
            {
                textForLink = value;
                OnPropertyChanged("TextForLink");
            }
        }

        private string htmlForPreview;

        public string HtmlForPreview
        {
            get { return htmlForPreview; }
            set
            {
                htmlForPreview = value;
                OnPropertyChanged("HtmlForPreview");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public EmailModel()
        {
            MessageToSend = new MimeMessage();
            BodyBuilder = new BodyBuilder();
        }
    }

}
