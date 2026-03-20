using PhishingApp.Model;
using System.ComponentModel.DataAnnotations;

namespace PhishingApp.Helpers
{
    public static class EmailHelper
    {
        public static void FormatBody(EmailModel _emailModel)
        {
            if (_emailModel.HtmlBody == null)
            {
                _emailModel.HtmlBody = "\n" + "<p  style=\"white-space: pre-line\">" + _emailModel.Body + "</p>" + "\n";
                _emailModel.HtmlBodyHelper = _emailModel.Body;

                _emailModel.HtmlForPreview = "\n" + "<p  style=\"white-space: pre-line\">" + _emailModel.Body + "</p>" + "\n";
            }
            else
            {
                string temp = _emailModel.Body.Substring(_emailModel.HtmlBodyHelper.Length);

                if (temp != "")
                {
                    _emailModel.HtmlBody += "\n" + "<p  style=\"white-space: pre-line\">" + temp + " </p>" + "\n";

                    _emailModel.HtmlForPreview += "\n" + "<p  style=\"white-space: pre-line\">" + temp + " </p>" + "\n";
                    _emailModel.HtmlBodyHelper = _emailModel.Body;
                }
            }
        }

        public static bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
